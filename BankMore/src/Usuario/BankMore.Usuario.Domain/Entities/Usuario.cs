using BankMore.Domain.Messages;
using System.Text.RegularExpressions;

namespace BankMore.Domain.Entities;

public sealed class Usuario
{
    public Guid Id { get; private set; }
    public string Cpf { get; private set; }
    public string SenhaHash { get; private set; }
    public bool Ativo { get; private set; } = true;

    protected Usuario() { }

    public Usuario(string cpf, string senha)
    {
        cpf = Regex.Replace(cpf ?? "", "[^0-9]", "");

        if (!CpfEhValido(cpf))
            throw new ArgumentException(UsuarioMensagens.CpfInvalido, nameof(cpf));

        if (string.IsNullOrWhiteSpace(senha) || senha.Length < 6)
            throw new ArgumentException(UsuarioMensagens.SenhaInvalida, nameof(senha));

        Id = Guid.NewGuid();
        Cpf = cpf;
        SenhaHash = GerarHash(senha);
    }

    public bool ValidarSenha(string senha)
    {
        return SenhaHash.Equals(GerarHash(senha), StringComparison.Ordinal);
    }

    public void Inativar() => Ativo = false;

    private static string GerarHash(string senha)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(senha);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public static bool CpfEhValido(string cpf)
    {
        cpf = Regex.Replace(cpf ?? "", "[^0-9]", "");
        if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
            return false;

        var multiplicadores1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicadores2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        var soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * multiplicadores1[i]).Sum();
        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        tempCpf += digito1;
        soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * multiplicadores2[i]).Sum();
        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith($"{digito1}{digito2}");
    }
}
