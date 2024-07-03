using ControladorPagamento.Entities.Shared;

namespace ControladorPagamento.Entities;

public class Produto : EntityBase
{
    public double Preco { get; set; }
    public string Descricao { get; set; } = null!;
}
