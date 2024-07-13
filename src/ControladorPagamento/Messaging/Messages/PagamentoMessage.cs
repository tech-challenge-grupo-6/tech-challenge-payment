

namespace ControladorPagamento.Messaging.Messages;

public class PagamentoMessage
{
    public string IdPedido { get; set; }
    public bool Status {  get; set; }   

}

