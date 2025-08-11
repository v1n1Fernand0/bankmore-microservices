using BankMore.Domain.Entities;

namespace BankMore.Application.Cache;

public interface ITarifaCache
{
    Task<List<Tarifa>> ObterTarifasAsync(Guid idContaCorrente);
    Task ArmazenarTarifasAsync(Guid idContaCorrente, List<Tarifa> tarifas);
}
