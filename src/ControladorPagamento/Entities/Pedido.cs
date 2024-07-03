using ControladorPagamento.Entities.Shared;
using ControladorPagamento.Messaging.Messages;

namespace ControladorPagamento.Entities;

public class Pedido : EntityBase
{
    public Status Status { get; set; } = Status.Criado;
    public Guid? ClienteId { get; set; }
    public virtual ICollection<Produto> Produtos { get; set; } = null!;
    public double ValorTotal { get; set; }
    public virtual Pagamento? Pagamento { get; set; }
    public bool Pago { get; set; } = false;

    public Pagamento GerarPagamento(MetodoPagamento metodoPagamento)
    {
        Pagamento = new Pagamento
        {
            PedidoId = Id,
            Valor = ValorTotal,
            MetodoPagamento = metodoPagamento,
            Status = Status.Recebido
        };

        return Pagamento;
    }
}
