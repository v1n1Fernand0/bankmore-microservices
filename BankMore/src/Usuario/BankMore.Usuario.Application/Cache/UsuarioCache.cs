using BankMore.Application.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace BankMore.Application.Cache;

public class UsuarioCache : IUsuarioCache
{
    private readonly ILogger<UsuarioCache> _logger;

    private readonly ConcurrentDictionary<Guid, (Guid IdConta, string NumeroConta)> _cache = new();

    private readonly ConcurrentDictionary<string, (string Cpf, Guid IdUsuario)> _cachePorNumeroConta = new();

    public UsuarioCache(ILogger<UsuarioCache> logger)
    {
        _logger = logger;
    }

    public Task AtualizarContaAsync(Guid idUsuario, Guid idConta, string numeroConta, string cpf)
    {
        _cache[idUsuario] = (idConta, numeroConta);
        _cachePorNumeroConta[numeroConta] = (cpf, idUsuario);

        _logger.LogInformation($"Cache atualizado: Usuário {idUsuario}, Conta {numeroConta}, CPF {cpf}");
        return Task.CompletedTask;
    }

    public Task AtualizarContaAsync(Guid idUsuario, Guid idConta, string numeroConta)
    {
        throw new NotImplementedException();
    }

    public Task<(Guid IdConta, string NumeroConta)?> ObterContaAsync(Guid idUsuario)
    {
        if (_cache.TryGetValue(idUsuario, out var conta))
        {
            return Task.FromResult<(Guid IdConta, string NumeroConta)?>(conta);
        }
        return Task.FromResult<(Guid IdConta, string NumeroConta)?>(null);
    }

    public Task<(string Cpf, Guid IdUsuario)?> ObterContaAsyncPorNumero(string numeroConta)
    {
        if (_cachePorNumeroConta.TryGetValue(numeroConta, out var resultado))
        {
            return Task.FromResult<(string Cpf, Guid IdUsuario)?>(resultado);
        }
        return Task.FromResult<(string Cpf, Guid IdUsuario)?>(null);
    }
}
