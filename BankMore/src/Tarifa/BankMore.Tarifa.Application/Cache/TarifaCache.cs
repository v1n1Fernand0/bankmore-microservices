using BankMore.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace BankMore.Application.Cache;

public class TarifaCache : ITarifaCache
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _expiracao;

    public TarifaCache(IMemoryCache cache, IConfiguration config)
    {
        _cache = cache;
        var minutos = int.Parse(config.GetSection("Cache:TarifaCacheExpirationMinutes").Value ?? "0"); 
        _expiracao = TimeSpan.FromMinutes(minutos);
    }

    public Task<List<Tarifa>> ObterTarifasAsync(Guid idContaCorrente)
    {
        _cache.TryGetValue(idContaCorrente, out List<Tarifa>? tarifas);
        return Task.FromResult(tarifas ?? new List<Tarifa>());
    }

    public Task ArmazenarTarifasAsync(Guid idContaCorrente, List<Tarifa> tarifas)
    {
        _cache.Set(idContaCorrente, tarifas, _expiracao);
        return Task.CompletedTask;
    }
}
