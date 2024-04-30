using ControladorPedidos.App.Entities;
using ControladorPedidos.App.Entities.Repositories;
using ControladorPedidos.App.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace ControladorPedidos.App.Gateways;

public class UsuarioRepository(DatabaseContext dbContext) : IUsuarioRepository
{
  public async Task<Usuario?> GetByLoginAndPassword(string login, string password)
  {
    return await dbContext.Usuarios.FirstOrDefaultAsync(p => p.Login == login && p.Senha == password);
  }

  public void Add(Usuario Usuario)
  {
    dbContext.Usuarios.Add(Usuario);
    dbContext.SaveChanges();
  }

  public void Update(Usuario Usuario)
  {
    dbContext.Usuarios.Update(Usuario);
    dbContext.SaveChanges();
  }
}