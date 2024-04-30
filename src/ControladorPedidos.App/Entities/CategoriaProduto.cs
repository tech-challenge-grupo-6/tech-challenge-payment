using ControladorPedidos.App.Entities.Shared;

namespace ControladorPedidos.App.Entities;

public class CategoriaProduto : EntityBase
{
    public string Nome { get; set; } = null!;
    public virtual ICollection<Produto> Produtos { get; set; } = null!;
}
