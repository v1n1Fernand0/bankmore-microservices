using BankMore.Application.Common;
using BankMore.Application.Dtos;
using BankMore.Domain.Enums;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Commands.Transferir;

public sealed class TransferirHandler : IRequestHandler<TransferirCommand, Result<TransferenciaDto>>
{
    private readonly IContaCorrenteRepository _contas;

    public TransferirHandler(IContaCorrenteRepository contas)
    {
        _contas = contas;
    }

    public async Task<Result<TransferenciaDto>> Handle(TransferirCommand request, CancellationToken ct)
    {
        if (request.Valor <= 0)
            return Result<TransferenciaDto>.Fail("Valor da transferência deve ser maior que zero.");

        if (request.IdContaOrigem == request.IdContaDestino)
            return Result<TransferenciaDto>.Fail("Conta de origem e destino devem ser diferentes.");

        var origem = await _contas.ObterPorIdAsync(request.IdContaOrigem);
        var destino = await _contas.ObterPorIdAsync(request.IdContaDestino);

        if (origem is null || destino is null)
            return Result<TransferenciaDto>.Fail("Conta de origem ou destino não encontrada.");

        if (!origem.Ativo || !destino.Ativo)
            return Result<TransferenciaDto>.Fail("Conta de origem ou destino está inativa.");

        if (request.Valor > origem.Saldo)
            return Result<TransferenciaDto>.Fail("Saldo insuficiente na conta de origem.");

        origem.RegistrarMovimento(request.Valor, TipoMovimento.Debito, request.Data);
        destino.RegistrarMovimento(request.Valor, TipoMovimento.Credito, request.Data);

        await _contas.AtualizarAsync(origem);
        await _contas.AtualizarAsync(destino);

        var dto = new TransferenciaDto(
            origem.IdContaCorrente,
            destino.IdContaCorrente,
            request.Valor,
            request.Data,
            origem.Saldo,
            destino.Saldo
        );

        return Result<TransferenciaDto>.Success(dto);
    }
}
