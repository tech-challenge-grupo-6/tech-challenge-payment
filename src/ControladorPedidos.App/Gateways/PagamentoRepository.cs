using ControladorPedidos.App.Entities;
using ControladorPedidos.App.Entities.Repositories;
using ControladorPedidos.App.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace ControladorPedidos.App.Gateways;

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
