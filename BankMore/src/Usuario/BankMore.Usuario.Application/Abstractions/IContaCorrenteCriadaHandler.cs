using BankMore.Application.Events;

namespace BankMore.Application.EventHandlers;

public interface IContaCorrenteCriadaHandler
{
    Task ProcessarAsync(ContaCorrenteCriadaEvent evento);
}
