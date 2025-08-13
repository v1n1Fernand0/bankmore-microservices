namespace BankMore.Transferencia.Domain.ValueObjects;

using BankMore.Transferencia.Domain.Exceptions;

public readonly record struct RequisicaoId
{
    public string Value { get; }

    public RequisicaoId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Identificação da requisição obrigatória.");
        Value = value;
    }

    public override string ToString() => Value;
}
