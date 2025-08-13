using BankMore.Transferencia.Application.Commands.DoTransfer;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace BankMore.Transferencia.Tests.Unit.Validators;

public class DoTransferValidatorTests
{
    private readonly DoTransferValidator _validator = new();

    [Fact]
    public void Deve_aprovar_comando_valido()
    {
        var cmd = new DoTransferCommand(
            IdempotencyKey: "key-123",
            RequisicaoId: "req-456",
            IdContaOrigem: "origem-789",
            ContaDestino: 123456,
            Valor: 10.25m);

        var result = _validator.TestValidate(cmd);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Deve_rejeitar_valor_invalido(decimal valor)
    {
        var cmd = new DoTransferCommand("k", "r", "o", 1, valor);
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Valor);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Deve_rejeitar_conta_destino_invalida(long conta)
    {
        var cmd = new DoTransferCommand("k", "r", "o", conta, 100);
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.ContaDestino);
    }

    [Fact]
    public void Deve_rejeitar_quando_faltar_idempotency_key()
    {
        var cmd = new DoTransferCommand("", "r", "o", 1, 10);
        var result = _validator.TestValidate(cmd);
        result.ShouldHaveValidationErrorFor(x => x.IdempotencyKey);
    }
}
