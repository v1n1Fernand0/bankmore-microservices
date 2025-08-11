using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using BankMore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankMore.Infrastructure.Data.Repositories
{
    public class TarifaRepository : ITarifaRepository
    {
        private readonly TarifaDbContext _context;

        public TarifaRepository(TarifaDbContext context)
        {
            _context = context;
        }

        public async Task<Tarifa> AdicionarAsync(Tarifa tarifa, CancellationToken cancellationToken)
        {
            _context.Tarifas.Add(tarifa);
            await _context.SaveChangesAsync(cancellationToken);
            return tarifa;
        }

        public async Task<List<Tarifa>> ObterPorContaAsync(Guid idContaCorrente, CancellationToken cancellationToken)
        {
            return await _context.Tarifas
                .Where(t => t.IdContaCorrente == idContaCorrente)
                .OrderByDescending(t => t.DataMovimento)
                .ToListAsync(cancellationToken);
        }

        public async Task<Tarifa?> ObterPorIdAsync(Guid idTarifa, CancellationToken cancellationToken)
        {
            return await _context.Tarifas
                .FirstOrDefaultAsync(t => t.IdTarifa == idTarifa, cancellationToken);
        }
    }
}
