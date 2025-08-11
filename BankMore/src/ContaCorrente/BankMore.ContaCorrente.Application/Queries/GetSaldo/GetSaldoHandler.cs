using BankMore.Application.Common;
using BankMore.Application.Dtos;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Queries;

public sealed class GetSaldoHandler : IRequestHandler<GetSaldoQuery, Result<SaldoDto>>
{
    private readonly IContaCorrenteRepository _contas;

    public GetSaldoHandler(IContaCorrenteRepository contas) => _contas = contas;

    public async Task<Result<SaldoDto>> Handle(GetSaldoQuery request, CancellationToken ct)
    {
        if (request.IdContaCorrente == Guid.Empty)
            return Result<SaldoDto>.Fail("ID da conta corrente é inválido.");

        var conta = await _contas.ObterPorIdAsync(request.IdContaCorrente);
        if (conta is null)
            return Result<SaldoDto>.Fail($"Conta com ID {request.IdContaCorrente} não encontrada.");

        var dto = new SaldoDto(conta.IdContaCorrente, conta.Saldo);
        return Result<SaldoDto>.Success(dto);
    }
}
