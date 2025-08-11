using BankMore.Domain.Entities;

namespace BankMore.Domain.Interfaces;

public interface IIdempotenciaRepository
{
    Task<Idempotencia?> ObterPorChaveAsync(Guid chaveIdempotencia);
    Task AdicionarAsync(Idempotencia idempotencia);
}
