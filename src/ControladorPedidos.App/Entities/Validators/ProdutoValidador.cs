namespace ControladorPedidos.App.Entities.Validators;

public class ProdutoValidador : IValidador<Produto>
{
    public static bool IsValid(Produto produto)
    {
        if (string.IsNullOrEmpty(produto.Nome))
            throw new ArgumentException("Nome do produto não pode ser vazio");

        if (produto.CategoriaId == Guid.Empty)
            throw new ArgumentException("Categoria do produto não pode ser vazio");

        if (produto.Preco == 0)
            throw new ArgumentException("Preço do produto não pode ser 0");

        if (string.IsNullOrEmpty(produto.Descricao))
            throw new ArgumentException("Descrição do produto não pode ser vazio");

        if (string.IsNullOrEmpty(produto.Imagem))
            throw new ArgumentException("Imagem do produto não pode ser vazio");

        return true;
    }
}
