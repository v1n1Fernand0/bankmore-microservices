using FluentValidation;

namespace BankMore.Transferencia.Application.Commands.DoTransfer;

public sealed class DoTransferValidator : AbstractValidator<DoTransferCommand>
{
    public DoTransferValidator()
    {
        RuleFor(x => x.IdempotencyKey).NotEmpty().MaximumLength(128);
        RuleFor(x => x.RequisicaoId).NotEmpty().MaximumLength(128);
        RuleFor(x => x.IdContaOrigem).NotEmpty().MaximumLength(64);

        RuleFor(x => x.ContaDestino).GreaterThan(0);
        RuleFor(x => x.Valor).GreaterThan(0m);
    }
}
