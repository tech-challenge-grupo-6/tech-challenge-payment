using ControladorPedidos.App.Entities.Shared;

namespace ControladorPedidos.App.Entities;

public class Pedido : EntityBase
{
    public virtual Cliente? Cliente { get; set; } = null!;
    public Guid? ClienteId { get; set; }
    public Status Status { get; set; } = Status.Criado;
    public virtual ICollection<Produto> Produtos { get; set; } = null!;
    public double ValorTotal { get; set; }
    public virtual Pagamento? Pagamento { get; set; }
    public Guid? PagamentoId { get; set; }

    public Pagamento GerarPagamento(MetodoPagamento metodoPagamento)
    {
        ValorTotal = Produtos.Sum(p => p.Preco);
        Status = Status.Recebido;

        Pagamento = new Pagamento
        {
            Pedido = this,
            PedidoId = Id,
            Valor = ValorTotal,
            MetodoPagamento = metodoPagamento
        };

        return Pagamento;
    }
}
