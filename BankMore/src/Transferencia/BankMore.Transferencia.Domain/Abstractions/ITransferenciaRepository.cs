namespace BankMore.Transferencia.Domain.Abstractions;

using TransferenciaEntity = Entities.Transferencia;

public interface ITransferenciaRepository
{
    Task RegistrarAsync(TransferenciaEntity transferencia, CancellationToken ct);
}
