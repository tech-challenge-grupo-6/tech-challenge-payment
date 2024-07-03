using ControladorPagamento.Entities.Shared;
using ControladorPagamento.Entities;

namespace ControladorPagamento.Messaging.Messages;

public record PedidoMessage()
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public Status Status { get; set; } = Status.Criado;
    public string ClienteId { get; set; } = string.Empty;
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    public double ValorTotal { get; set; }
    public bool Pago { get; set; } = false;

}