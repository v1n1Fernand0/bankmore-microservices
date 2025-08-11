using BankMore.Application.Abstractions;
using BankMore.Application.Events;

namespace BankMore.Application.EventHandlers
{
    public class ContaCorrenteCriadaHandler : IContaCorrenteCriadaHandler
    {
        private readonly IUsuarioCache _cache;

        public ContaCorrenteCriadaHandler(IUsuarioCache cache)
        {
            _cache = cache;
        }

        public async Task ProcessarAsync(ContaCorrenteCriadaEvent evento)
        {
            await _cache.AtualizarContaAsync(evento.IdUsuario, evento.IdConta, evento.NumeroConta);
        }
    }

}
