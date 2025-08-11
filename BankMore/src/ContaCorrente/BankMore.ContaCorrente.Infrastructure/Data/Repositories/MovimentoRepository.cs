using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using BankMore.Infrastructure.Persistence;

namespace BankMore.Infrastructure.Data.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly ContaCorrenteDbContext _context;
        public MovimentoRepository(ContaCorrenteDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarAsync(Movimento movimento)
        {
            await _context.Movimentos.AddAsync(movimento);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<Movimento>> ObterPorContaAsync(Guid idContaCorrente)
        {
            return Task.FromResult(_context.Movimentos
                .Where(m => m.IdContaCorrente == idContaCorrente)
                .AsEnumerable());
        }
    }

}
