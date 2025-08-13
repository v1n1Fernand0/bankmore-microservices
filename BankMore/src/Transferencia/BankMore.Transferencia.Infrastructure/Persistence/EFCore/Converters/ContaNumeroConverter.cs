using BankMore.Transferencia.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankMore.Transferencia.Infrastructure.Persistence.EFCore.Converters;

public sealed class ContaNumeroConverter : ValueConverter<ContaNumero, long>
{
    public ContaNumeroConverter() : base(v => v.Value, v => new ContaNumero(v)) { }
}
