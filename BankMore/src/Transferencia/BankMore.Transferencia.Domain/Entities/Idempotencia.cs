namespace BankMore.Transferencia.Domain.Entities;

public sealed class Idempotencia
{
    public string Chave { get; }
    public string? Requisicao { get; }
    public string? Resultado { get; private set; }

    public Idempotencia(string chave, string? requisicao, string? resultado)
        => (Chave, Requisicao, Resultado) = (chave, requisicao, resultado);

    public void AtualizarResultado(string resultado) => Resultado = resultado;
}
