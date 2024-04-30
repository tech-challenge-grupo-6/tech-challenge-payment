namespace ControladorPedidos.App.Entities.Validators;

public class PedidoValidador : IValidador<Pedido>
{
    public static bool IsValid(Pedido pedido)
    {
        if (pedido.ClienteId == Guid.Empty)
            throw new ArgumentException("Id do pedido não pode ser vazio");

        if (pedido.Produtos.Count == 0)
            throw new ArgumentException("Lista de produtos do pedido não pode ser vazia");

        return true;
    }
}
