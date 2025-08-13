using System.Security.Claims;
using BankMore.Transferencia.Application.Commands.DoTransfer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankMore.Transferencia.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class TransferenciasController : ControllerBase
{
    private readonly IMediator _mediator;
    public TransferenciasController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Post(
        [FromBody] DoTransferRequest body,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,  
        CancellationToken ct = default)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
            return Forbid();

        var idContaOrigem = User.FindFirstValue("idcontacorrente")
                           ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(idContaOrigem))
            return BadRequest(new { type = "TOKEN_WITHOUT_ACCOUNT_ID" });

        var key = string.IsNullOrWhiteSpace(idempotencyKey) ? body.IdempotencyKey : idempotencyKey;
        if (string.IsNullOrWhiteSpace(key))
            return BadRequest(new { type = "MISSING_IDEMPOTENCY_KEY" });

        var cmd = new DoTransferCommand(
            IdempotencyKey: key,
            RequisicaoId: body.RequisicaoId,
            IdContaOrigem: idContaOrigem,
            ContaDestino: body.ContaDestino,
            Valor: body.Valor);

        await _mediator.Send(cmd, ct);
        return NoContent();
    }
}

public sealed record DoTransferRequest(
    string? IdempotencyKey,
    string RequisicaoId,
    long ContaDestino,
    decimal Valor
);
