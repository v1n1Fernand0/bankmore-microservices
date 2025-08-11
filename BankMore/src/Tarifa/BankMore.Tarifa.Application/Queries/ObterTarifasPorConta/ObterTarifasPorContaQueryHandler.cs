using BankMore.Application.Cache;
using BankMore.Application.DTOs;
using BankMore.Application.Queries.ObterTarifasPorConta;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Handlers;

public class ObterTarifasPorContaQueryHandler : IRequestHandler<ObterTarifasPorContaQuery, List<TarifaDto>>
{
    private readonly ITarifaRepository _repository;
    private readonly ITarifaCache _cache;

    public ObterTarifasPorContaQueryHandler(ITarifaRepository repository, ITarifaCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<List<TarifaDto>> Handle(ObterTarifasPorContaQuery request, CancellationToken cancellationToken)
    {
        var tarifas = await _cache.ObterTarifasAsync(request.IdContaCorrente);

        if (tarifas.Count == 0)
        {
            tarifas = await _repository.ObterPorContaAsync(request.IdContaCorrente, cancellationToken);
            await _cache.ArmazenarTarifasAsync(request.IdContaCorrente, tarifas);
        }

        return tarifas.Select(t => new TarifaDto
        {
            IdTarifa = t.IdTarifa,
            IdContaCorrente = t.IdContaCorrente,
            Valor = t.Valor,
            Descricao = t.Descricao,
            DataMovimento = t.DataMovimento
        }).ToList();
    }
}
