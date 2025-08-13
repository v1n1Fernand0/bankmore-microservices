using BankMore.Transferencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankMore.Transferencia.Infrastructure.Persistence.EFCore.Configurations;

public sealed class IdempotenciaConfig : IEntityTypeConfiguration<Idempotencia>
{
    public void Configure(EntityTypeBuilder<Idempotencia> b)
    {
        b.ToTable("idempotencia");
        b.HasKey(x => x.Chave);

        b.Property(x => x.Chave)
            .HasColumnName("chave_idempotencia")
            .HasMaxLength(37)
            .IsRequired();

        b.Property(x => x.Requisicao)
            .HasColumnName("requisicao")
            .HasMaxLength(1000);

        b.Property(x => x.Resultado)
            .HasColumnName("resultado")
            .HasMaxLength(1000);
    }
}
