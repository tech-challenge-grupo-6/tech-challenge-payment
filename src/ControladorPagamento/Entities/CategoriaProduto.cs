using ControladorPagamento.Entities.Shared;

namespace ControladorPagamento.Entities;

public class CategoriaProduto : EntityBase
{
    public string Nome { get; set; } = null!;
    public virtual ICollection<Produto> Produtos { get; set; } = null!;
}
