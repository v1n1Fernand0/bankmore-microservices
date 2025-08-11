using BankMore.Application.Commands;
using FluentValidation;

namespace BankMore.Application.Validators
{
    public class CriarTarifaCommandValidator : AbstractValidator<CriarTarifaCommand>
    {
        public CriarTarifaCommandValidator()
        {
            RuleFor(x => x.IdContaCorrente)
                .NotEmpty().WithMessage("O ID da conta corrente é obrigatório.");

            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("O valor da tarifa deve ser maior que zero.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(200).WithMessage("A descrição deve ter no máximo 200 caracteres.");
        }
    }
}
