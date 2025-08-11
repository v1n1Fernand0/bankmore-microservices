using BankMore.Application.Common;
using BankMore.Application.Dtos;
using MediatR;

namespace BankMore.Application.Queries.GetContaPorNumero;

/// <summary>
/// Consulta para obter os dados de uma conta corrente pelo número.
/// </summary>
public sealed record GetContaPorNumeroQuery(string Numero)
    : IRequest<Result<ContaCorrenteDto>>;
