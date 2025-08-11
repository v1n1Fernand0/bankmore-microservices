
using BankMore.Domain.Entities;

namespace BankMore.Domain.Interfaces
{
    public interface ITarifaRepository
    {
        Task<Tarifa> AdicionarAsync(Tarifa tarifa, CancellationToken cancellationToken);
        Task<List<Tarifa>> ObterPorContaAsync(Guid idContaCorrente, CancellationToken cancellationToken);
        Task<Tarifa?> ObterPorIdAsync(Guid idTarifa, CancellationToken cancellationToken);
    }
}


