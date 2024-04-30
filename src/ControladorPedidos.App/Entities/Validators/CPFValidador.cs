using System.Text.RegularExpressions;

namespace ControladorPedidos.App.Entities.Validators;

public class CPFValidador
{
  public static bool ValidarCpf(string cpf)
  {
    if (string.IsNullOrWhiteSpace(cpf))
      return false;

    try
    {
      // Remove caracteres não numéricos do CPF
      cpf = Regex.Replace(cpf, @"[^\d]", "");

      // Verifica se o CPF possui 11 dígitos
      if (cpf.Length != 11)
        return false;

      // Verifica se todos os dígitos do CPF são iguais
      if (new string(cpf[0], 11) == cpf)
        return false;

      // Calcula o primeiro dígito verificador do CPF
      int soma = 0;
      for (int i = 0; i < 9; i++)
        soma += (cpf[i] - '0') * (10 - i);
      int resto = soma % 11;
      int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

      // Calcula o segundo dígito verificador do CPF
      soma = 0;
      for (int i = 0; i < 10; i++)
        soma += (cpf[i] - '0') * (11 - i);
      resto = soma % 11;
      int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

      // Verifica se os dígitos verificadores do CPF são válidos
      return cpf.EndsWith(digitoVerificador1.ToString() + digitoVerificador2.ToString());
    }
    catch
    {
      return false;
    }
  }
}
