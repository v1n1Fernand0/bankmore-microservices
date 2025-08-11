using BankMore.Application.Common;
using BankMore.Application.Dtos;
using BankMore.Application.Mapping;
using BankMore.Domain.Enums;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Commands;

public sealed class DepositarHandler : IRequestHandler<DepositarCommand, Result<ContaCorrenteDto>>
{
    private readonly IContaCorrenteRepository _contas;

    public DepositarHandler(IContaCorrenteRepository contas) => _contas = contas;

    public async Task<Result<ContaCorrenteDto>> Handle(DepositarCommand request, CancellationToken ct)
    {
        var conta = await _contas.ObterPorIdAsync(request.IdContaCorrente);
        if (conta is null)
            return Result<ContaCorrenteDto>.Fail("Conta não encontrada.");

        if (!conta.Ativo)
            return Result<ContaCorrenteDto>.Fail("Conta está inativa.");

        if (request.Valor <= 0)
            return Result<ContaCorrenteDto>.Fail("Valor do depósito deve ser maior que zero.");

        conta.RegistrarMovimento(request.Valor, TipoMovimento.Credito, request.Data);

        await _contas.AtualizarAsync(conta);

        return Result<ContaCorrenteDto>.Success(conta.ToDto());
    }
}
