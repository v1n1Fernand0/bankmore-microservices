namespace BankMore.Application.Dtos;

public sealed record TransferenciaDto(
    Guid IdContaOrigem,
    Guid IdContaDestino,
    decimal Valor,
    DateTime Data,
    decimal SaldoOrigem,
    decimal SaldoDestino
);
