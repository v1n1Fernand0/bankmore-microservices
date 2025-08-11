using BankMore.Application.Common;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Commands.RegistrarMovimento;

public sealed class RegistrarMovimentoHandler : IRequestHandler<RegistrarMovimentoCommand, Result<bool>>
{
    private readonly IContaCorrenteRepository _contas;

    public RegistrarMovimentoHandler(IContaCorrenteRepository contas)
    {
        _contas = contas;
    }

    public async Task<Result<bool>> Handle(RegistrarMovimentoCommand request, CancellationToken ct)
    {
        var conta = await _contas.ObterPorIdAsync(request.IdConta);
        if (conta is null)
            return Result<bool>.Fail("Conta não encontrada.");

        if (!conta.Ativo)
            return Result<bool>.Fail("Conta está inativa.");

        if (request.Valor <= 0)
            return Result<bool>.Fail("Valor do movimento deve ser maior que zero.");

        if (request.Tipo == Domain.Enums.TipoMovimento.Debito && request.Valor > conta.Saldo)
            return Result<bool>.Fail("Saldo insuficiente.");

        conta.RegistrarMovimento(request.Valor, request.Tipo, request.Data);
        await _contas.AtualizarAsync(conta);

        return Result<bool>.Success(true);
    }
}
