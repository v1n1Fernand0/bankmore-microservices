using BankMore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankMore.Infrastructure.Persistence;

public class UsuarioDbContext : DbContext
{
    public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("Usuarios"); 

            e.HasKey(u => u.Id);

            e.Property(u => u.Cpf)
                .IsRequired()
                .HasMaxLength(11);

            e.HasIndex(u => u.Cpf)
                .IsUnique();

            e.Property(u => u.SenhaHash)
                .IsRequired();

            e.Property(u => u.Ativo)
                .IsRequired();
        });
    }
}
