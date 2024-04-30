using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using ControladorPedidos.App.Gateways.DependencyInjection;
using ControladorPedidos.App.Infrastructure.DataBase.DependencyInjection;
using ControladorPedidos.App.UseCases.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ControladorPedidos", Version = "v1" });
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
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ControladorPedidos.App.xml"));
});

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddRepositories();
builder.Services.AddUseCases();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddHttpClient();
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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
