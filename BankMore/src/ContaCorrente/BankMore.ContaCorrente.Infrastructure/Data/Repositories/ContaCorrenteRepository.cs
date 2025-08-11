using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using BankMore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankMore.Infrastructure.Data.Repositories
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly ContaCorrenteDbContext _context;

        public ContaCorrenteRepository(ContaCorrenteDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(ContaCorrente conta)
        {
            await _context.ContasCorrentes.AddAsync(conta);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(ContaCorrente conta)
        {
            _context.ContasCorrentes.Update(conta);
            await _context.SaveChangesAsync();
        }

        public async Task<ContaCorrente?> ObterPorIdAsync(Guid id)
        {
            return await _context.ContasCorrentes
                .Include(c => c.Movimentos)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdContaCorrente == id);
        }

        public async Task<ContaCorrente?> ObterPorNumeroAsync(string numero)
        {
            return await _context.ContasCorrentes
                .Include(c => c.Movimentos)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Numero == numero);
        }
    }
}
