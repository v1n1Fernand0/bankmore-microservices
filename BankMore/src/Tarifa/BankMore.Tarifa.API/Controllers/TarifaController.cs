using BankMore.Application.Commands;
using BankMore.Application.DTOs;
using BankMore.Application.Queries.ObterTarifasPorConta;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BankMore.Tarifa.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize]
    public class TarifaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TarifaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] CriarTarifaCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            return CreatedAtAction(nameof(ObterPorConta), new { idContaCorrente = command.IdContaCorrente }, result);
        }

        [HttpGet("{idContaCorrente:guid}")]
        [ProducesResponseType(typeof(List<TarifaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorConta([FromRoute] Guid idContaCorrente, CancellationToken ct)
        {
            var result = await _mediator.Send(new ObterTarifasPorContaQuery(idContaCorrente), ct);
            return result.Any() ? Ok(result) : NotFound(new { error = "Nenhuma tarifa encontrada para esta conta." });
        }
    }
}
