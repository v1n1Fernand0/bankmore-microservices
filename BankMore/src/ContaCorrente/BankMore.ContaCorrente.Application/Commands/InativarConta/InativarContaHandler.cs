using BankMore.Application.Common;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Commands;

public sealed class InativarContaHandler : IRequestHandler<InativarContaCommand, Result<bool>>
{
    private readonly IContaCorrenteRepository _contas;

    public InativarContaHandler(IContaCorrenteRepository contas)
    {
        _contas = contas;
    }

    public async Task<Result<bool>> Handle(InativarContaCommand request, CancellationToken ct)
    {
        var conta = await _contas.ObterPorIdAsync(request.IdContaCorrente);
        if (conta is null)
            return Result<bool>.Fail("Conta não encontrada.");

        if (!conta.Ativo)
            return Result<bool>.Fail("Conta já está inativa.");

        conta.Inativar();
        await _contas.AtualizarAsync(conta);

        return Result<bool>.Success(true);
    }
}
