using MediatR;

namespace BankMore.Transferencia.Application.Commands.DoTransfer;

public sealed record DoTransferCommand(
    string IdempotencyKey,
    string RequisicaoId,
    string IdContaOrigem,  
    long ContaDestino,
    decimal Valor
) : IRequest<Unit>;
