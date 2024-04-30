using System.Text.RegularExpressions;

namespace ControladorPedidos.App.Entities.Validators;

public static class EmailValidador
{
    public static bool ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Define a expressão regular para validar o e-mail
            string expressaoRegular = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            // Cria um objeto Regex com a expressão regular
            Regex regex = new(expressaoRegular);

            // Verifica se o e-mail corresponde à expressão regular
            return regex.IsMatch(email);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
