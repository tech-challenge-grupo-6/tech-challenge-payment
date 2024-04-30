namespace ControladorPedidos.App.Entities.Repositories;

public interface IUsuarioRepository
{
  Task<Usuario?> GetByLoginAndPassword(string login, string password);

  void Add(Usuario usuario);
  void Update(Usuario usuario);
}