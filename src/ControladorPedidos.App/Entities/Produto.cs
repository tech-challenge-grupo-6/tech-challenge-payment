using ControladorPedidos.App.Entities.Shared;

namespace ControladorPedidos.App.Entities;

public class Produto : EntityBase
{
    public string Nome { get; set; } = null!;
    public virtual CategoriaProduto Categoria { get; set; } = null!;
    public Guid CategoriaId { get; set; }
    public double Preco { get; set; }
    public string Descricao { get; set; } = null!;
    public string Imagem { get; set; } = null!;
    public virtual ICollection<Pedido> Pedidos { get; set; } = null!;
}
