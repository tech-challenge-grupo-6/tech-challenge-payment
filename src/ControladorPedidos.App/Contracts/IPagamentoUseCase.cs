using ControladorPedidos.App.Entities;
using ControladorPedidos.App.Entities.Shared;

namespace ControladorPedidos.App.Contracts;

public interface IPagamentoUseCase
{
    Task EfetuarMercadoPagoQRCodeAsync(Guid pedidoId, string? token);
    Task<Pagamento> ConsultarPagamentoPeloPedido(Guid pedidoId);
    Task<Guid?> ConcluirPagamento(Guid pedidoId, bool aprovado, string? motivo, string? token);
    Task<bool> ObterStatusDoPedidoAsync(Guid pedidoId);
    Task<Pedido?> ObterPedidoPorIdAsync(Guid pedidoId, string? token);
}
