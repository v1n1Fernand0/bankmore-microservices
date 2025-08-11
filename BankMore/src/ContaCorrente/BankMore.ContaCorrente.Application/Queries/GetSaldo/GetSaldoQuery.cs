using BankMore.Application.Common;
using BankMore.Application.Dtos;
using MediatR;

namespace BankMore.Application.Queries;

/// <summary>
/// Consulta para obter o saldo atual de uma conta corrente.
/// </summary>
public sealed record GetSaldoQuery(Guid IdContaCorrente)
    : IRequest<Result<SaldoDto>>;
