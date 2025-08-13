using BankMore.Transferencia.Application.Commands.DoTransfer;
using BankMore.Transferencia.Domain.Abstractions;
using BankMore.Transferencia.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankMore.Transferencia.Tests.Unit.Commands;

public class DoTransferHandlerTests
{
    private static DoTransferHandler CreateHandler(
        Mock<IIdempotenciaStore>? idem = null,
        Mock<IContaCorrenteClient>? conta = null,
        Mock<ITransferenciaRepository>? repo = null,
        Mock<ILogger<DoTransferHandler>>? log = null)
    {
        idem ??= new Mock<IIdempotenciaStore>(MockBehavior.Strict);
        conta ??= new Mock<IContaCorrenteClient>(MockBehavior.Strict);
        repo ??= new Mock<ITransferenciaRepository>(MockBehavior.Strict);
        log ??= new Mock<ILogger<DoTransferHandler>>();

        return new DoTransferHandler(idem.Object, conta.Object, repo.Object, log.Object);
    }

    private static DoTransferCommand Cmd() => new(
        IdempotencyKey: Guid.NewGuid().ToString(),
        RequisicaoId: Guid.NewGuid().ToString(),
        IdContaOrigem: "conta-origem-123",
        ContaDestino: 9876543210,
        Valor: 100.50m);

    [Fact]
    public async Task Sucesso_deve_debitar_creditar_persistir_e_gravar_idempotencia()
    {
        var idem = new Mock<IIdempotenciaStore>(MockBehavior.Strict);
        var conta = new Mock<IContaCorrenteClient>(MockBehavior.Strict);
        var repo = new Mock<ITransferenciaRepository>(MockBehavior.Strict);

        var cmd = Cmd();

        idem.Setup(x => x.ObterResultadoAsync(cmd.IdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        conta.Setup(x => x.DebitarAsync(It.IsAny<RequisicaoId>(), It.IsAny<Dinheiro>(), It.IsAny<CancellationToken>()))
             .Returns(Task.CompletedTask);
        conta.Setup(x => x.CreditarAsync(It.IsAny<RequisicaoId>(), It.IsAny<ContaNumero>(), It.IsAny<Dinheiro>(), It.IsAny<CancellationToken>()))
             .Returns(Task.CompletedTask);

        repo.Setup(x => x.RegistrarAsync(It.IsAny<BankMore.Transferencia.Domain.Entities.Transferencia>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        idem.Setup(x => x.SalvarAsync(cmd.IdempotencyKey, It.Is<string>(s => s.Contains("\"IdContaOrigem\"")), It.Is<string>(s => s.Contains("\"ok\"")), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler(idem, conta, repo);

        await handler.Handle(cmd, CancellationToken.None);

        conta.Verify(x => x.DebitarAsync(It.IsAny<RequisicaoId>(), It.IsAny<Dinheiro>(), It.IsAny<CancellationToken>()), Times.Once);
        conta.Verify(x => x.CreditarAsync(It.IsAny<RequisicaoId>(), It.IsAny<ContaNumero>(), It.IsAny<Dinheiro>(), It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(x => x.RegistrarAsync(It.IsAny<BankMore.Transferencia.Domain.Entities.Transferencia>(), It.IsAny<CancellationToken>()), Times.Once);
        idem.Verify(x => x.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Idempotencia_hit_deve_encerrar_sem_chamar_dependencias()
    {
        var idem = new Mock<IIdempotenciaStore>(MockBehavior.Strict);
        var conta = new Mock<IContaCorrenteClient>(MockBehavior.Strict);
        var repo = new Mock<ITransferenciaRepository>(MockBehavior.Strict);

        var cmd = Cmd();

        idem.Setup(x => x.ObterResultadoAsync(cmd.IdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync("{\"status\":\"ok\"}");

        var handler = CreateHandler(idem, conta, repo);

        await handler.Handle(cmd, CancellationToken.None);

        conta.VerifyNoOtherCalls();
        repo.VerifyNoOtherCalls();
        idem.Verify(x => x.ObterResultadoAsync(cmd.IdempotencyKey, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Falha_no_credito_deve_realizar_estorno_debito_na_conta_do_token_e_propagar_erro()
    {
        var idem = new Mock<IIdempotenciaStore>(MockBehavior.Strict);
        var conta = new Mock<IContaCorrenteClient>(MockBehavior.Strict);
        var repo = new Mock<ITransferenciaRepository>(MockBehavior.Strict);

        var cmd = Cmd();

        idem.Setup(x => x.ObterResultadoAsync(cmd.IdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string?)null);

        conta.Setup(x => x.DebitarAsync(It.IsAny<RequisicaoId>(), It.IsAny<Dinheiro>(), It.IsAny<CancellationToken>()))
             .Returns(Task.CompletedTask);

        conta.Setup(x => x.CreditarAsync(It.IsAny<RequisicaoId>(), It.IsAny<ContaNumero>(), It.IsAny<Dinheiro>(), It.IsAny<CancellationToken>()))
             .ThrowsAsync(new InvalidOperationException("falha de crédito"));

        conta.Setup(x => x.DebitarMinhaContaAsync(It.IsAny<RequisicaoId>(), It.IsAny<Dinheiro>(), It.IsAny<CancellationToken>()))
             .Returns(Task.CompletedTask);

        var handler = CreateHandler(idem, conta, repo);

        var act = async () => await handler.Handle(cmd, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();

        conta.Verify(x => x.DebitarAsync(It.IsAny<RequisicaoId>(), It.IsAny<Dinheiro>(), It.IsAny<CancellationToken>()), Times.Once);
        conta.Verify(x => x.CreditarAsync(It.IsAny<RequisicaoId>(), It.IsAny<ContaNumero>(), It.IsAny<Dinheiro>(), It.IsAny<CancellationToken>()), Times.Once);
        conta.Verify(x => x.DebitarMinhaContaAsync(It.IsAny<RequisicaoId>(), It.IsAny<Dinheiro>(), It.IsAny<CancellationToken>()), Times.Once);

        repo.Verify(x => x.RegistrarAsync(It.IsAny<Domain.Entities.Transferencia>(), It.IsAny<CancellationToken>()), Times.Never);
        idem.Verify(x => x.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
