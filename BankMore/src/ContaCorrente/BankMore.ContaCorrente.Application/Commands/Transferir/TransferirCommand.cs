using BankMore.Application.Common;
using BankMore.Application.Dtos;
using MediatR;

namespace BankMore.Application.Commands.Transferir;

public sealed record TransferirCommand(
    Guid IdContaOrigem,
    Guid IdContaDestino,
    decimal Valor,
    DateTime Data,
    Guid RequestId
) : IRequest<Result<TransferenciaDto>>;
