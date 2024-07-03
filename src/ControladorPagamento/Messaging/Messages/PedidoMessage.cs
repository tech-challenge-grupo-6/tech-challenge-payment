using ControladorPagamento.Entities.Shared;
using ControladorPagamento.Entities;

namespace ControladorPagamento.Messaging.Messages;

public record PedidoMessage()
{
    public string Id { get; set; }
    public Status Status { get; set; } = Status.Criado;
    public string ClienteId { get; set; }
    public ICollection<Produto> Produtos { get; set; }
    public double ValorTotal { get; set; }
    public bool Pago { get; set; } = false;

}