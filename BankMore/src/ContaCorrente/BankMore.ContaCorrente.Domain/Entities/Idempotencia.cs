using static BankMore.Domain.Messages.DomainMessages;

namespace BankMore.Domain.Entities;

public class Idempotencia
{
    public Guid ChaveIdempotencia { get; private set; }
    public string Requisicao { get; private set; } = string.Empty;
    public string Resultado { get; private set; } = string.Empty;

    protected Idempotencia() { }

    public Idempotencia(Guid chaveIdempotencia, string requisicao, string resultado)
    {
        if (chaveIdempotencia == Guid.Empty)
            throw new ArgumentException(IdempotenciaMensagens.ChaveInvalida, nameof(chaveIdempotencia));

        if (string.IsNullOrWhiteSpace(requisicao))
            throw new ArgumentException(IdempotenciaMensagens.RequisicaoVazia, nameof(requisicao));

        if (string.IsNullOrWhiteSpace(resultado))
            throw new ArgumentException(IdempotenciaMensagens.ResultadoVazio, nameof(resultado));

        ChaveIdempotencia = chaveIdempotencia;
        Requisicao = requisicao;
        Resultado = resultado;
    }
}
