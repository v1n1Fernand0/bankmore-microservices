namespace BankMore.Transferencia.Domain.ValueObjects;

using BankMore.Transferencia.Domain.Exceptions;

public readonly record struct ContaNumero
{
    public long Value { get; }

    public ContaNumero(long value)
    {
        if (value <= 0) throw new DomainException("Número da conta inválido.");
        Value = value;
    }

    public override string ToString() => Value.ToString();
}
