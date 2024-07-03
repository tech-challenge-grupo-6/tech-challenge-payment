using ControladorPagamento.Application.Responses;
using ControladorPagamento.Entities;
using MediatR;

namespace ControladorPagamento.Application.Commands;

public class PagamentoWebhookCommand : IRequest<bool>
{
    public Pedido Pedido { get; set; }
    public bool Aprovado { get; set; }
    public string? Motivo { get; set; }
}
