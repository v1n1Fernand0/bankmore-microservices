using BankMore.Application.DTOs;

namespace BankMore.Application.Interfaces
{
    public interface ITarifaApplicationService
    {
        Task<Guid> CriarTarifaAsync(CriarTarifaDto dto, CancellationToken cancellationToken);
        Task<List<TarifaDto>> ObterPorContaAsync(Guid idContaCorrente, CancellationToken cancellationToken);
    }
}
