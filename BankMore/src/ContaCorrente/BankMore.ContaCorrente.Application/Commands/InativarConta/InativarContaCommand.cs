using BankMore.Application.Common;
using MediatR;

namespace BankMore.Application.Commands;

public sealed record InativarContaCommand(Guid IdContaCorrente)
    : IRequest<Result<bool>>;
