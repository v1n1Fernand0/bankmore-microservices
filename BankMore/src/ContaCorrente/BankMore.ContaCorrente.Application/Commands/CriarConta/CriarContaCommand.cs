using BankMore.Application.Common;
using BankMore.Application.Dtos;
using MediatR;

namespace BankMore.Application.Commands;

/// <summary>
/// Comando para criar uma nova conta corrente.
/// </summary>
public sealed record CriarContaCommand(
    string Numero,
    Guid IdUsuario,
    string Nome,
    string Senha
) : IRequest<Result<ContaCorrenteDto>>;
