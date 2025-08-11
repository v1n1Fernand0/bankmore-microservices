using BankMore.Application.Common;
using BankMore.Application.Dtos;
using BankMore.Application.Mapping;
using BankMore.Domain.Enums;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Commands;

public sealed class DebitarHandler : IRequestHandler<DebitarCommand, Result<ContaCorrenteDto>>
{
    private readonly IContaCorrenteRepository _contas;

    public DebitarHandler(IContaCorrenteRepository contas) => _contas = contas;

    public async Task<Result<ContaCorrenteDto>> Handle(DebitarCommand request, CancellationToken ct)
    {
        var conta = await _contas.ObterPorIdAsync(request.IdContaCorrente);
        if (conta is null)
            return Result<ContaCorrenteDto>.Fail("Conta não encontrada.");

        if (!conta.Ativo)
            return Result<ContaCorrenteDto>.Fail("Conta está inativa.");

        if (request.Valor <= 0)
            return Result<ContaCorrenteDto>.Fail("Valor do débito deve ser maior que zero.");

        if (request.Valor > conta.Saldo)
            return Result<ContaCorrenteDto>.Fail("Saldo insuficiente.");

        conta.RegistrarMovimento(request.Valor, TipoMovimento.Debito, request.Data);

        await _contas.AtualizarAsync(conta);

        return Result<ContaCorrenteDto>.Success(conta.ToDto());
    }
}
