using ControladorPagamento.Entities;
using ControladorPagamento.Entities.Repositories;
using ControladorPagamento.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace ControladorPagamento.Gateways;

public class PagamentoRepository(DatabaseContext dbContext) : IPagamentoRepository
{
    public async Task Add(Pagamento pagamento)
    {
        dbContext.Pagamentos.Add(pagamento);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Pagamento?> GetByPedidoId(Guid pedidoId)
    {
        return await dbContext.Pagamentos.FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
    }
}
