using BankMore.Application.Common;
using BankMore.Application.Dtos;
using MediatR;

namespace BankMore.Application.Queries;

public sealed record GetMovimentosQuery(Guid IdContaCorrente)
    : IRequest<Result<IReadOnlyCollection<MovimentoDto>>>;
