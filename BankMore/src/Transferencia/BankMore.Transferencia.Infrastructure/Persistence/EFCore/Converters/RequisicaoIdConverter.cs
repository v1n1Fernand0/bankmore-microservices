using BankMore.Transferencia.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankMore.Transferencia.Infrastructure.Persistence.EFCore.Converters;

public sealed class RequisicaoIdConverter : ValueConverter<RequisicaoId, string>
{
    public RequisicaoIdConverter() : base(v => v.Value, v => new RequisicaoId(v)) { }
}
