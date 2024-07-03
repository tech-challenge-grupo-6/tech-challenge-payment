using ControladorPagamento.Messaging.Messages;

namespace ControladorPagamento.Messaging.Producers;

public interface IMessageSender
{
    Task SendMessageAsync(PedidoMessage message, string fila);
}
