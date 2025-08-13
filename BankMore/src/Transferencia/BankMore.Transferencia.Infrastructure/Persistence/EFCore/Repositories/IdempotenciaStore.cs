using BankMore.Transferencia.Domain.Abstractions;
using BankMore.Transferencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankMore.Transferencia.Infrastructure.Persistence.EFCore.Repositories;

public sealed class IdempotenciaStore : IIdempotenciaStore
{
    private readonly TransferenciaDbContext _db;
    public IdempotenciaStore(TransferenciaDbContext db) => _db = db;

    public async Task<string?> ObterResultadoAsync(string chave, CancellationToken ct)
        => await _db.Idempotencias
            .AsNoTracking()
            .Where(x => x.Chave == chave)
            .Select(x => x.Resultado)
            .FirstOrDefaultAsync(ct);

    public async Task SalvarAsync(string chave, string requisicao, string resultado, CancellationToken ct)
    {
        var idem = await _db.Idempotencias.FindAsync([chave], ct);
        if (idem is null)
        {
            idem = new Idempotencia(chave, requisicao, resultado);
            _db.Idempotencias.Add(idem);
        }
        else
        {
            idem.AtualizarResultado(resultado);
        }
        await _db.SaveChangesAsync(ct);
    }
}
