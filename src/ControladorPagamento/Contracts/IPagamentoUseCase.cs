using ControladorPagamento.Entities;

namespace ControladorPagamento.Contracts;

public interface IPagamentoUseCase
{
    Task EfetuarMercadoPagoQRCodeAsync(Pedido pedido);
    Task<Pagamento> ConsultarPagamentoPeloPedido(Guid pedidoId);
    Task<Guid?> ConcluirPagamento(Pedido pedido, bool aprovado, string? motivo);
    Task<bool> ObterStatusDoPedidoAsync(Guid pedidoId);
    Task<Pedido?> ObterPedidoPorIdAsync(Guid pedidoId, string? token);
}
