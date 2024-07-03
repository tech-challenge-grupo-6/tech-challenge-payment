

namespace ControladorPagamento.Messaging.Messages;

public class PagamentoMessage
{
    public Guid IdPedido { get; set; }
    public bool status {  get; set; }   

}

