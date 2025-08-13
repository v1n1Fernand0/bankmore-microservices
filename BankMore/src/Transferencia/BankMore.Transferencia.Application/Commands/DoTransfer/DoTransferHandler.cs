using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using BankMore.Transferencia.Domain.Abstractions;
using BankMore.Transferencia.Domain.ValueObjects;
using TransferenciaEntity = BankMore.Transferencia.Domain.Entities.Transferencia;

namespace BankMore.Transferencia.Application.Commands.DoTransfer;

public sealed class DoTransferHandler : IRequestHandler<DoTransferCommand, Unit>
{
    private readonly IIdempotenciaStore _idempotencia;
    private readonly IContaCorrenteClient _conta;
    private readonly ITransferenciaRepository _repo;
    private readonly ILogger<DoTransferHandler> _log;

    public DoTransferHandler(
        IIdempotenciaStore idempotencia,
        IContaCorrenteClient conta,
        ITransferenciaRepository repo,
        ILogger<DoTransferHandler> log)
        => (_idempotencia, _conta, _repo, _log) = (idempotencia, conta, repo, log);

    public async Task<Unit> Handle(DoTransferCommand cmd, CancellationToken ct)
    {
        var cache = await _idempotencia.ObterResultadoAsync(cmd.IdempotencyKey, ct);
        if (cache is not null)
        {
            _log.LogInformation("Idempotência atingida. key={Key}", cmd.IdempotencyKey);
            return Unit.Value;
        }

        var reqId = new RequisicaoId(cmd.RequisicaoId);
        var contaDestino = new ContaNumero(cmd.ContaDestino);
        var valor = new Dinheiro(cmd.Valor);

        await _conta.DebitarAsync(reqId, valor, ct);

        try
        {
            await _conta.CreditarAsync(reqId, contaDestino, valor, ct);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Falha ao creditar destino. Estorno (débito) na conta do token conforme enunciado.");
            await _conta.DebitarMinhaContaAsync(reqId, valor, ct);
            throw;
        }

        var transferencia = TransferenciaEntity.Criar(
            idContaOrigem: cmd.IdContaOrigem,
            contaDestino: contaDestino,
            valor: valor,
            nowUtc: DateTime.UtcNow);

        await _repo.RegistrarAsync(transferencia, ct);

        var reqJson = JsonSerializer.Serialize(cmd);
        await _idempotencia.SalvarAsync(cmd.IdempotencyKey, reqJson, "{\"status\":\"ok\"}", ct);

        return Unit.Value;
    }
}
