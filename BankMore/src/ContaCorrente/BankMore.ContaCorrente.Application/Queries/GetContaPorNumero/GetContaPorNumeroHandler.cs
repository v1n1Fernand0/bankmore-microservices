using BankMore.Application.Common;
using BankMore.Application.Dtos;
using BankMore.Application.Mapping;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Queries.GetContaPorNumero;

public sealed class GetContaPorNumeroHandler : IRequestHandler<GetContaPorNumeroQuery, Result<ContaCorrenteDto>>
{
    private readonly IContaCorrenteRepository _contas;

    public GetContaPorNumeroHandler(IContaCorrenteRepository contas)
    {
        _contas = contas;
    }

    public async Task<Result<ContaCorrenteDto>> Handle(GetContaPorNumeroQuery request, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(request.Numero))
            return Result<ContaCorrenteDto>.Fail("Informe o número da conta.");

        var conta = await _contas.ObterPorNumeroAsync(request.Numero);
        if (conta is null)
            return Result<ContaCorrenteDto>.Fail($"Conta com número {request.Numero} não encontrada.");

        return Result<ContaCorrenteDto>.Success(conta.ToDto());
    }
}
