using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Services.AddLogging();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapPost("/pagar", async ([FromServices] IConfiguration configuration, [FromServices] HttpClient httpClient, [FromServices] ILogger<Program> logger, [FromBody] PagamentoDto pagamentoDto) =>
{
    logger.LogInformation("Efetuando pagamento do pedido {PedidoId}", pagamentoDto.PedidoId);
    int taxaAprovacao = configuration.GetValue("TaxaAprovacao", 100);
    string? webhookUrl = configuration.GetValue<string>("WebhookUrl");

    int resultado = Random.Shared.Next(100);
    if (resultado < taxaAprovacao)
    {
        logger.LogInformation("Pagamento do pedido {PedidoId} aprovado", pagamentoDto.PedidoId);
        await httpClient.PostAsJsonAsync(webhookUrl, new PagamentoResult(pagamentoDto.PedidoId, true, "Pagamento aprovado com sucesso"));
        return Results.Ok();
    }
    else
    {
        logger.LogInformation("Pagamento do pedido {PedidoId} reprovado", pagamentoDto.PedidoId);
        await httpClient.PostAsJsonAsync(webhookUrl, new PagamentoResult(pagamentoDto.PedidoId, false, "Pagamento reprovado"));
        return Results.Problem("Pagamento reprovado");
    }
})
.WithName("Pagamento")
.WithOpenApi();

app.Run();

record PagamentoDto(Guid PedidoId, decimal Valor, Guid ClienteId);
record PagamentoResult(Guid PedidoId, bool Aprovado, string? Motivo);
