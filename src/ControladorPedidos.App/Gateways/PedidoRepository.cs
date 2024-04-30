using ControladorPedidos.App.Entities;
using ControladorPedidos.App.Entities.Repositories;
using ControladorPedidos.App.Entities.Shared;
using ControladorPedidos.App.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace ControladorPedidos.App.Gateways;

public class PedidoRepository(DatabaseContext dbContext) : IPedidoRepository
{
  public async Task<IEnumerable<Pedido?>> GetAll()
  {
    return await dbContext.Pedidos.Include(p => p.Produtos).ToListAsync();
  }

  public async Task<IEnumerable<Pedido?>> GetByStatus(Status status)
  {
    return await dbContext.Pedidos.Where(p => p.Status == status).ToListAsync();
  }

  public async Task<IEnumerable<Pedido?>> GetByCliente(Guid clienteId)
  {
    return await dbContext.Pedidos.Where(p => p.ClienteId == clienteId).ToListAsync();
  }

  public async Task<Pedido?> GetById(Guid id)
  {
    return await dbContext
      .Pedidos
      .Include(p => p.Produtos)
      .FirstOrDefaultAsync(p => p.Id == id);
  }

  public async Task Add(Pedido Pedido)
  {
    dbContext.Pedidos.Add(Pedido);
    await dbContext.SaveChangesAsync();
  }

  public async Task UpdateStatus(Pedido pedido)
  {
    dbContext.Pedidos.Update(pedido);
    await dbContext.SaveChangesAsync();
  }
}