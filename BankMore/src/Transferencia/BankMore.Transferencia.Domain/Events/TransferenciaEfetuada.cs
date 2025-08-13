namespace BankMore.Transferencia.Domain.Events;

public sealed record TransferenciaEfetuada(
    string IdTransferencia,
    string IdContaOrigem,
    long NumeroContaDestino,
    decimal Valor,
    DateTime DataMovimentoUtc
);
