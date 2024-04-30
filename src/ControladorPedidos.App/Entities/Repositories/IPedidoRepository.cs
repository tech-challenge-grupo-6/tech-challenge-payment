using ControladorPedidos.App.Entities.Shared;

namespace ControladorPedidos.App.Entities.Repositories;

public interface IPedidoRepository
{
  Task<IEnumerable<Pedido?>> GetAll();
  Task<IEnumerable<Pedido?>> GetByStatus(Status status);
  Task<IEnumerable<Pedido?>> GetByCliente(Guid clientId);
  Task<Pedido?> GetById(Guid id);

  Task UpdateStatus(Pedido pedido);
  Task Add(Pedido pedido);
}