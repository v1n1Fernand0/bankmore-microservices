using BankMore.Domain.Entities;

namespace BankMore.Domain.Interfaces;

public interface IContaCorrenteRepository
{
    Task<ContaCorrente?> ObterPorNumeroAsync(string numero);
    Task<ContaCorrente?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(ContaCorrente conta);
    Task AtualizarAsync(ContaCorrente conta);
}
