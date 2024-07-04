using ControladorPagamento.Messaging.Messages;

namespace ControladorPagamento.Messaging.Producers;

public interface IMessageSender
{
    Task SendMessageAsync(Object message, string fila);
}
