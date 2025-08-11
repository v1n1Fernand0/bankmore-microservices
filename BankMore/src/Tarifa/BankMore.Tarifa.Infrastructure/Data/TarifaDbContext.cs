using BankMore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankMore.Infrastructure.Persistence
{
    public class TarifaDbContext : DbContext
    {
        public TarifaDbContext(DbContextOptions<TarifaDbContext> options)
            : base(options) { }

        public DbSet<Tarifa> Tarifas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarifa>(entity =>
            {
                entity.ToTable("tarifa");

                entity.HasKey(t => t.IdTarifa);

                entity.Property(t => t.IdTarifa)
                      .HasColumnName("idtarifa")
                      .HasColumnType("TEXT")
                      .IsRequired();

                entity.Property(t => t.IdContaCorrente)
                      .HasColumnName("idcontacorrente")
                      .HasColumnType("TEXT")
                      .IsRequired();

                entity.Property(t => t.DataMovimento)
                      .HasColumnName("datamovimento")
                      .HasColumnType("TEXT")
                      .IsRequired();

                entity.Property(t => t.Valor)
                      .HasColumnName("valor")
                      .HasColumnType("REAL")
                      .IsRequired();
            });
        }
    }
}
