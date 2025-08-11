using Microsoft.EntityFrameworkCore;

namespace BankMore.Transferencia.Infrastructure.Persistence
{
    public class TransferenciaDbContext : DbContext
    {
        public TransferenciaDbContext(DbContextOptions<TransferenciaDbContext> options)
            : base(options) { }

        // Adicione seus DbSets aqui quando tiver entidades
        // public DbSet<Transferencia> Transferencias { get; set; }
    }
}
