using BankMore.Domain.Enums;
using static BankMore.Domain.Messages.DomainMessages;

namespace BankMore.Domain.Entities;

public sealed class Movimento
{
    public Guid IdMovimento { get; private set; }
    public Guid IdContaCorrente { get; private set; }
    public DateTime Data { get; private set; }
    public TipoMovimento TipoMovimento { get; private set; }
    public decimal Valor { get; private set; }

    protected Movimento() { }

    public Movimento(
        Guid idMovimento,
        Guid idContaCorrente,
        DateTime data,
        TipoMovimento tipoMovimento,
        decimal valor)
    {
        if (idMovimento == Guid.Empty)
            throw new ArgumentException(MovimentoMensagens.IdInvalido, nameof(idMovimento));

        if (idContaCorrente == Guid.Empty)
            throw new ArgumentException(MovimentoMensagens.ContaObrigatoria, nameof(idContaCorrente));

        if (data == default)
            throw new ArgumentException(MovimentoMensagens.DataObrigatoria, nameof(data));

        if (!Enum.IsDefined(typeof(TipoMovimento), tipoMovimento))
            throw new ArgumentException(MovimentoMensagens.TipoInvalido, nameof(tipoMovimento));

        if (valor <= 0)
            throw new ArgumentException(MovimentoMensagens.ValorInvalido, nameof(valor));

        IdMovimento = idMovimento;
        IdContaCorrente = idContaCorrente;
        Data = data;
        TipoMovimento = tipoMovimento;
        Valor = valor;
    }
}
