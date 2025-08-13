namespace BankMore.Transferencia.Domain.Abstractions;

public interface IIdempotenciaStore
{
    Task<string?> ObterResultadoAsync(string chave, CancellationToken ct);
    Task SalvarAsync(string chave, string requisicao, string resultado, CancellationToken ct);
}
