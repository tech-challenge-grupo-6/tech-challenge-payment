using ControladorPagamento.Entities;
using MediatR;

namespace ControladorPagamento.Application.Commands;

public class PagamentoWebhookCommand : IRequest<bool>
{
    public Pedido Pedido { get; set; } = new Pedido();
    public bool Aprovado { get; set; }
    public string? Motivo { get; set; }
}
