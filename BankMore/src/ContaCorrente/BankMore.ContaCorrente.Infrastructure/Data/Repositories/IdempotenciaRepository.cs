using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using BankMore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankMore.Infrastructure.Data.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly ContaCorrenteDbContext _context;

        public IdempotenciaRepository(ContaCorrenteDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Idempotencia idempotencia)
        {
            await _context.Idempotencias.AddAsync(idempotencia);
            await _context.SaveChangesAsync();
        }

        public async Task<Idempotencia?> ObterPorChaveAsync(Guid chaveIdempotencia)
        {
            return await _context.Idempotencias
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.ChaveIdempotencia == chaveIdempotencia);
        }
    }
}
