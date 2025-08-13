using BankMore.Transferencia.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankMore.Transferencia.Infrastructure.Persistence.EFCore.Converters;

public sealed class DinheiroConverter : ValueConverter<Dinheiro, decimal>
{
    public DinheiroConverter() : base(v => v.Value, v => new Dinheiro(v)) { }
}
