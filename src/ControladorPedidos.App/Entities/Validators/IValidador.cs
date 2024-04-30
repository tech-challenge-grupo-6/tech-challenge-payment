namespace ControladorPedidos.App.Entities.Validators;

public interface IValidador<T>
{
    static abstract bool IsValid(T entidade);
}
