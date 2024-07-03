using AutoMapper;
using ControladorPagamento.Messaging.Messages;

namespace ControladorPagamento.Entities.Profiles;

public class PedidoProfile : Profile
{
    public PedidoProfile()
    {
        CreateMap<PedidoMessage, Pedido>().ReverseMap();
    }
}
