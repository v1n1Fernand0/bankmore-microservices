using BankMore.Application.Events;

namespace BankMore.Application.Abstractions
{
    public interface IContaCorrenteEventPublisher
    {
        Task PublicarAsync(ContaCorrenteCriadaEvent evento);
    }
}
