using BankMore.Application.Common;
using BankMore.Application.Dtos;
using MediatR;

namespace BankMore.Application.Commands;

/// <summary>
/// Comando para debitar um valor da conta corrente.
/// </summary>
public sealed record DebitarCommand(
    Guid IdContaCorrente,
    decimal Valor,
    DateTime Data
) : IRequest<Result<ContaCorrenteDto>>;
