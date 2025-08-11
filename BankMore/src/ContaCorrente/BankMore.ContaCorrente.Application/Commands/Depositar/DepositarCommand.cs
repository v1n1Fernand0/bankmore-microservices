using BankMore.Application.Common;
using BankMore.Application.Dtos;
using MediatR;

namespace BankMore.Application.Commands;

/// <summary>
/// Comando para depositar um valor na conta corrente.
/// </summary>
public sealed record DepositarCommand(
    Guid IdContaCorrente,
    decimal Valor,
    DateTime Data
) : IRequest<Result<ContaCorrenteDto>>;
