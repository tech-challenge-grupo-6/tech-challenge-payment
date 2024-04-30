using ControladorPedidos.App.Entities.Shared;

namespace ControladorPedidos.App.Entities;

public class Pagamento : EntityBase
{
    public virtual Pedido Pedido { get; set; } = null!;
    public Guid PedidoId { get; set; }
    public double Valor { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
}
