using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransferenciaEntity = BankMore.Transferencia.Domain.Entities.Transferencia;
using BankMore.Transferencia.Infrastructure.Persistence.EFCore.Converters;

namespace BankMore.Transferencia.Infrastructure.Persistence.EFCore.Configurations;

public sealed class TransferenciaConfig : IEntityTypeConfiguration<TransferenciaEntity>
{
    public void Configure(EntityTypeBuilder<TransferenciaEntity> b)
    {
        b.ToTable("transferencia");
        b.HasKey(x => x.IdTransferencia);

        b.Property(x => x.IdTransferencia)
            .HasColumnName("idtransferencia")
            .HasMaxLength(37)
            .IsRequired();

        b.Property(x => x.IdContaOrigem)
            .HasColumnName("idcontacorrente_origem")
            .HasMaxLength(37)
            .IsRequired();

        b.Property(x => x.ContaDestino)
            .HasConversion(new ContaNumeroConverter())
            .HasColumnName("idcontacorrente_destino")
            .IsRequired();

        b.Property(x => x.DataMovimentoUtc)
            .HasColumnName("datamovimento")
            .HasConversion(
                v => v.ToString("dd/MM/yyyy"),
                s => DateTime.SpecifyKind(
                        DateTime.ParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DateTimeKind.Utc))
            .IsRequired();

        b.Property(x => x.Valor)
            .HasConversion(new DinheiroConverter())
            .HasColumnName("valor")
            .IsRequired();
    }
}
