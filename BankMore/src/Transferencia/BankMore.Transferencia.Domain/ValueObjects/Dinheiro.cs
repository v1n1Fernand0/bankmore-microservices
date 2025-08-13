namespace BankMore.Transferencia.Domain.ValueObjects;

using BankMore.Transferencia.Domain.Exceptions;

public readonly record struct Dinheiro
{
    public decimal Value { get; }

    public Dinheiro(decimal value)
    {
        if (value <= 0) throw new DomainException("Valor deve ser positivo.");
        Value = decimal.Round(value, 2, MidpointRounding.ToZero);
    }

    public static implicit operator decimal(Dinheiro d) => d.Value;
    public override string ToString() => Value.ToString("0.00");
}
