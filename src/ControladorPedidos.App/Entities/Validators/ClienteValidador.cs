namespace ControladorPedidos.App.Entities.Validators;

public class ClienteValidador : IValidador<Cliente>
{
    public static bool IsValid(Cliente entidade)
    {
        if (string.IsNullOrEmpty(entidade.Nome))
            throw new ArgumentException("Nome não pode ser vazio");


        if (!EmailValidador.ValidarEmail(entidade.Email))
            throw new ArgumentException("Email inválido");

        if (!CPFValidador.ValidarCpf(entidade.Cpf))
            throw new ArgumentException("Cpf inválido");

        return true;
    }
}
