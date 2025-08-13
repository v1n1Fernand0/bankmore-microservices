namespace BankMore.Transferencia.Infrastructure.Persistence.EFCore;

using Microsoft.EntityFrameworkCore;
using TransferenciaEntity = Domain.Entities.Transferencia;
using IdempotenciaEntity = Domain.Entities.Idempotencia;

public sealed class TransferenciaDbContext : DbContext
{
    public TransferenciaDbContext(DbContextOptions<TransferenciaDbContext> options) : base(options) { }

    public DbSet<TransferenciaEntity> Transferencias => Set<TransferenciaEntity>();
    public DbSet<IdempotenciaEntity> Idempotencias => Set<IdempotenciaEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransferenciaDbContext).Assembly);
}
