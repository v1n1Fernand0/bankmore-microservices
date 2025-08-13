namespace BankMore.Transferencia.Infrastructure.Persistence.EFCore.Repositories;

using BankMore.Transferencia.Domain.Abstractions;
using TransferenciaEntity = Domain.Entities.Transferencia;

public sealed class TransferenciaRepository : ITransferenciaRepository
{
    private readonly TransferenciaDbContext _db;
    public TransferenciaRepository(TransferenciaDbContext db) => _db = db;

    public async Task RegistrarAsync(TransferenciaEntity transferencia, CancellationToken ct)
    {
        _db.Set<TransferenciaEntity>().Add(transferencia);
        await _db.SaveChangesAsync(ct);
    }
}
