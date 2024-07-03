using ControladorPagamento.Messaging.Messages;
using MassTransit;
using MassTransit.Serialization;
using System.Net.Mime;

namespace ControladorPagamento.Messaging.Producers;

public class MessageSender : IMessageSender
{

    private readonly IBus _bus;
    private readonly ILogger<MessageSender> _logger;
    public MessageSender(IBus bus, ILogger<MessageSender> logger)
    {
        _bus = bus;
        _logger = logger;
    }
    public async Task SendMessageAsync(PedidoMessage message, string fila)
    {
        _logger.LogInformation("Inicia o envio da mensagem para fila");

        Uri url = new Uri($"queue:{fila}");
        var endpoint = await _bus.GetSendEndpoint(url);

        await endpoint.Send(message);

        _logger.LogInformation("Finaliza envio da mensagem para fila");
    }
}
