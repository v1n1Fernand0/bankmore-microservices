using BankMore.Application.Common;
using BankMore.Application.Dtos;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Queries;

public sealed class GetMovimentosHandler : IRequestHandler<GetMovimentosQuery, Result<IReadOnlyCollection<MovimentoDto>>>
{
    private readonly IContaCorrenteRepository _contas;

    public GetMovimentosHandler(IContaCorrenteRepository contas)
    {
        _contas = contas;
    }

    public async Task<Result<IReadOnlyCollection<MovimentoDto>>> Handle(GetMovimentosQuery request, CancellationToken ct)
    {
        var conta = await _contas.ObterPorIdAsync(request.IdContaCorrente);
        if (conta is null)
            return Result<IReadOnlyCollection<MovimentoDto>>.Fail("Conta não encontrada.");

        var movimentos = conta.Movimentos
            .OrderByDescending(m => m.Data)
            .Select(m => new MovimentoDto(
                m.IdMovimento,
                m.Data,
                m.TipoMovimento.ToString(),
                m.Valor
            ))
            .ToList()
            .AsReadOnly();

        return Result<IReadOnlyCollection<MovimentoDto>>.Success(movimentos);
    }
}
