using BankMore.Domain.Entities;

namespace BankMore.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorCpfAsync(string cpf);
    Task AdicionarAsync(Usuario usuario);
    Task InativarAsync(Guid idUsuario);
}

