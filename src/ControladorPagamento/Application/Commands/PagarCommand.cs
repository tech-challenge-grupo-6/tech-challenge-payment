using ControladorPagamento.Entities;
using MediatR;

namespace ControladorPagamento.Application.Commands;

public class PagarCommand : IRequest<bool>
{ 
    public Pedido Pedido { get; set; } = new Pedido();
}
