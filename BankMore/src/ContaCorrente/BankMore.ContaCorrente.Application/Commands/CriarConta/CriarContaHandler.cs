using BankMore.Application.Common;
using BankMore.Application.Dtos;
using BankMore.Application.Events;
using BankMore.Application.Mapping;
using BankMore.Application.Abstractions;
using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Commands;

public sealed class CriarContaHandler : IRequestHandler<CriarContaCommand, Result<ContaCorrenteDto>>
{
    private readonly IContaCorrenteRepository _contas;
    private readonly IContaCorrenteEventPublisher _eventPublisher;

    public CriarContaHandler(
        IContaCorrenteRepository contas,
        IContaCorrenteEventPublisher eventPublisher)
    {
        _contas = contas;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result<ContaCorrenteDto>> Handle(CriarContaCommand request, CancellationToken ct)
    {
        var existente = await _contas.ObterPorNumeroAsync(request.Numero);
        if (existente is not null)
            return Result<ContaCorrenteDto>.Fail("Já existe conta com esse número.");

        var conta = new ContaCorrente(
            Guid.NewGuid(),
            request.IdUsuario,
            request.Numero,
            request.Nome
        );

        await _contas.AdicionarAsync(conta);

        await _eventPublisher.PublicarAsync(new ContaCorrenteCriadaEvent
        {
            NumeroConta = conta.Numero,
            IdUsuario = conta.IdUsuario ,
            IdConta = conta.IdContaCorrente,
        });

        return Result<ContaCorrenteDto>.Success(conta.ToDto());
    }
}
