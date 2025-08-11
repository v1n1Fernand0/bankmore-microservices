using BankMore.Application.Common;
using BankMore.Domain.Enums;
using MediatR;

namespace BankMore.Application.Commands.RegistrarMovimento;

/// <summary>
/// Comando para registrar um movimento (crédito ou débito) em uma conta corrente.
/// </summary>
public sealed record RegistrarMovimentoCommand(
    Guid IdConta,
    decimal Valor,
    TipoMovimento Tipo,
    DateTime Data
) : IRequest<Result<bool>>;
