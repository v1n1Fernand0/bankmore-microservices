using BankMore.Domain.Entities;

namespace BankMore.Domain.Interfaces;

public interface IMovimentoRepository
{
    Task<IEnumerable<Movimento>> ObterPorContaAsync(Guid idContaCorrente);
    Task AdicionarAsync(Movimento movimento);
}
