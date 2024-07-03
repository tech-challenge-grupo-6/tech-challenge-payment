using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ControladorPagamento.Application.UseCases.DependencyInjection;
using ControladorPagamento.Gateways.DependencyInjection;
using ControladorPagamento.Infrastructure.DataBase.DependencyInjection;
using ControladorPagamento.Messaging.Consumers;
using ControladorPagamento.Messaging.Producers;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ControladorPagamento", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            []
        }
    });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ControladorPagamento.xml"));
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMessageSender, MessageSender>();
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddRepositories();
builder.Services.AddUseCases();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
    {
        // get JsonWebKeySet from AWS
        var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
        // deserialize the result
        var keys = JsonSerializer.Deserialize<JsonWebKeySet>(json)!.Keys;
        foreach (var key in keys)
        {
            // Acessar as propriedades da chave
            Console.WriteLine(key); // Exemplo de acesso a uma propriedade, como o ID da chave
            // Faça o que for necessário com cada chave...
        }
        // cast the result to be the type expected by IssuerSigningKeyResolver
        return (IEnumerable<SecurityKey>)keys;
    },
        ValidIssuer = $"https://cognito-idp.{builder.Configuration["AWS:Region"]}.amazonaws.com/{builder.Configuration["AWS:UserPoolId"]}",
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["AWS:AppClientId"],
        ValidateAudience = true
    };
});


builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<PedidoConsumer>();
    x.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("us-east-1", h =>
        {
            h.AccessKey(builder.Configuration["AWS:AccessKey"]);
            h.SecretKey(builder.Configuration["AWS:SecretKey"]);
        });

        cfg.ReceiveEndpoint("pedido-criado", e =>
        {
            e.DefaultContentType = new ContentType("application/json");
            e.UseRawJsonDeserializer();
            e.PrefetchCount = 1;
            e.UseMessageRetry(r => r.Interval(2, 10));
            e.ConfigureConsumer<PedidoConsumer>(context);
            e.ConfigureConsumeTopology = false;
        });

        cfg.UseRawJsonSerializer(RawSerializerOptions.AddTransportHeaders | RawSerializerOptions.CopyHeaders);

    });
});
builder.Services.AddScoped<MessageSender>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

if (builder.Environment.IsEnvironment("SUT"))
    app.MapControllers().AllowAnonymous();
else
    app.MapControllers();


app.Run();
