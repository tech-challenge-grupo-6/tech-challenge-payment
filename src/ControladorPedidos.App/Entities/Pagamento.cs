using ControladorPedidos.App.Entities.Shared;

namespace ControladorPedidos.App.Entities;

public class Pagamento : EntityBase
{
    public Guid PedidoId { get; set; }
    public double Valor { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
    public Status Status { get; set; }
}
