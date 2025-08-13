namespace BankMore.Transferencia.Domain.Abstractions;

using BankMore.Transferencia.Domain.ValueObjects;

public interface IContaCorrenteClient
{
    Task DebitarAsync(RequisicaoId requisicaoId, Dinheiro valor, CancellationToken ct);
    Task CreditarAsync(RequisicaoId requisicaoId, ContaNumero contaDestino, Dinheiro valor, CancellationToken ct);

    Task DebitarMinhaContaAsync(RequisicaoId requisicaoId, Dinheiro valor, CancellationToken ct);
}
