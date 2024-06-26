﻿using ControladorPagamento.Entities.Shared;

namespace ControladorPagamento.Entities;

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
