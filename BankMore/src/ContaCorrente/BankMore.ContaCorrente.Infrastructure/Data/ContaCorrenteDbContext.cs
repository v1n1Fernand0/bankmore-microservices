using BankMore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankMore.Infrastructure.Persistence
{
    public class ContaCorrenteDbContext : DbContext
    {
        public ContaCorrenteDbContext(DbContextOptions<ContaCorrenteDbContext> options)
            : base(options) { }

        public DbSet<ContaCorrente> ContasCorrentes { get; set; }
        public DbSet<Movimento> Movimentos { get; set; }
        public DbSet<Idempotencia> Idempotencias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContaCorrenteDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
