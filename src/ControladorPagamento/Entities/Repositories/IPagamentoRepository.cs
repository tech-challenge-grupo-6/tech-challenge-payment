namespace ControladorPagamento.Entities.Repositories;

public interface IPagamentoRepository
{
    Task Add(Pagamento pagamento);
    Task<Pagamento?> GetByPedidoId(Guid pedidoId);
}
