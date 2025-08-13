namespace BankMore.Transferencia.Domain.Entities;

using BankMore.Transferencia.Domain.Exceptions;
using BankMore.Transferencia.Domain.ValueObjects;

public sealed class Transferencia
{
    public string IdTransferencia { get; private set; } = default!;
    public string IdContaOrigem { get; private set; } = default!;
    public ContaNumero ContaDestino { get; private set; }
    public Dinheiro Valor { get; private set; }
    public DateTime DataMovimentoUtc { get; private set; }

    private Transferencia() { }

    private Transferencia(
        string idTransferencia,
        string idContaOrigem,
        ContaNumero contaDestino,
        Dinheiro valor,
        DateTime dataMovimentoUtc)
    {
        IdTransferencia = idTransferencia;
        IdContaOrigem = string.IsNullOrWhiteSpace(idContaOrigem)
            ? throw new DomainException("Conta de origem obrigatória.")
            : idContaOrigem;
        ContaDestino = contaDestino;
        Valor = valor;
        DataMovimentoUtc = dataMovimentoUtc;
    }

    public static Transferencia Criar(string idContaOrigem, ContaNumero contaDestino, Dinheiro valor, DateTime? nowUtc = null)
    {
        if (string.IsNullOrWhiteSpace(idContaOrigem))
            throw new DomainException("Conta de origem obrigatória.");

        return new Transferencia(
            idTransferencia: Guid.NewGuid().ToString(),
            idContaOrigem: idContaOrigem,
            contaDestino: contaDestino,
            valor: valor,
            dataMovimentoUtc: nowUtc ?? DateTime.UtcNow
        );
    }
}
