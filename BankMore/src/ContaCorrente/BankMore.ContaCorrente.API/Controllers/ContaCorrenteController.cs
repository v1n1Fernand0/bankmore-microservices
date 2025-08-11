using BankMore.Application.Commands;
using BankMore.Application.Commands.Transferir;
using BankMore.Application.Dtos;
using BankMore.Application.Queries;
using BankMore.Application.Queries.GetContaPorNumero;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BankMore.ContaCorrente.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class ContaCorrenteController : ControllerBase
{
    private readonly IMediator _mediator;
    public ContaCorrenteController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{numero:int}")]
    [ProducesResponseType(typeof(ContaCorrenteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNumero([FromRoute] int numero, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetContaPorNumeroQuery(numero), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContaCorrenteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CriarContaCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess) return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetByNumero),
            new { numero = result.Value.Numero }, result.Value);
    }

    [HttpPost("{id:guid}/depositos")]
    [ProducesResponseType(typeof(ContaCorrenteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Depositar([FromRoute] Guid id, [FromBody] DepositarCommandBody body, CancellationToken ct)
    {
        var cmd = new DepositarCommand(id, body.Valor, body.Data);
        var result = await _mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpPost("{id:guid}/debitos")]
    [ProducesResponseType(typeof(ContaCorrenteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Debitar([FromRoute] Guid id, [FromBody] DebitarCommandBody body, CancellationToken ct)
    {
        var cmd = new DebitarCommand(id, body.Valor, body.Data);
        var result = await _mediator.Send(cmd, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("{id:guid}/saldo")]
    [ProducesResponseType(typeof(SaldoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSaldo([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetSaldoQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }

    [HttpPost("transferencias")]
    [ProducesResponseType(typeof(TransferenciaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Transferir([FromBody] TransferirCommandBody body, CancellationToken ct)
    {
        var command = new TransferirCommand(
            body.IdContaOrigem,
            body.IdContaDestino,
            body.Valor,
            body.Data,
            body.RequestId
        );

        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpPatch("{id:guid}/inativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Inativar([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new InativarContaCommand(id), ct);
        return result.IsSuccess ? NoContent() : BadRequest(new { error = result.Error });
    }

    [HttpGet("{id:guid}/movimentos")]
    [ProducesResponseType(typeof(List<MovimentoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMovimentos([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetMovimentosQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }

    public record TransferirCommandBody(
        Guid IdContaOrigem,
        Guid IdContaDestino,
        decimal Valor,
        DateTime Data,
        Guid RequestId
    );

    public record DepositarCommandBody(decimal Valor, DateTime Data);
    public record DebitarCommandBody(decimal Valor, DateTime Data);
}


