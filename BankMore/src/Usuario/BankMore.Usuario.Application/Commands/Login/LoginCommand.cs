using BankMore.Application.Common;
using MediatR;

namespace BankMore.Application.Commands.Login;

public sealed record LoginCommand(string Identificador, string Senha)
    : IRequest<Result<string>>;
