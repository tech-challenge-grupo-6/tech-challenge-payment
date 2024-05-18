using ControladorPagamento.Entities;
using ControladorPagamento.Entities.Shared;

namespace ControladorPagamento.Contracts;

public interface IPagamentoUseCase
{
    Task EfetuarMercadoPagoQRCodeAsync(Guid pedidoId, string? token);
    Task<Pagamento> ConsultarPagamentoPeloPedido(Guid pedidoId);
    Task<Guid?> ConcluirPagamento(Guid pedidoId, bool aprovado, string? motivo, string? token);
    Task<bool> ObterStatusDoPedidoAsync(Guid pedidoId);
    Task<Pedido?> ObterPedidoPorIdAsync(Guid pedidoId, string? token);
}
