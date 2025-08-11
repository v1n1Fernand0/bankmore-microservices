using BankMore.Application.Common;
using MediatR;

namespace BankMore.Application.Commands;

public sealed record CadastrarUsuarioCommand(string Cpf, string Senha)
    : IRequest<Result<Guid>>;
