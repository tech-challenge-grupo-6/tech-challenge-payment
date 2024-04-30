namespace ControladorPedidos.App.Entities.Validators;

public class CategoriaProdutoValidador : IValidador<CategoriaProduto>
{
  public static bool IsValid(CategoriaProduto entidade)
  {
    if (string.IsNullOrEmpty(entidade.Nome))
      throw new ArgumentException("Nome não pode ser vazio");

    return true;
  }
}