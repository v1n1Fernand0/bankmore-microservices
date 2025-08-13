using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BankMore.Transferencia.Domain.Abstractions;
using BankMore.Transferencia.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace BankMore.Transferencia.Infrastructure.HttpClients;

public sealed class ContaCorrenteClient : IContaCorrenteClient
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _ctx;

    public ContaCorrenteClient(HttpClient http, IHttpContextAccessor ctx)
        => (_http, _ctx) = (http, ctx);

    private void ForwardBearer()
    {
        var headers = _ctx.HttpContext?.Request?.Headers;
        if (headers is null) return;
        if (headers.TryGetValue("Authorization", out var auth) && !string.IsNullOrWhiteSpace(auth))
            _http.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(auth.ToString());
    }

    public async Task DebitarAsync(RequisicaoId req, Dinheiro valor, CancellationToken ct)
    {
        ForwardBearer();
        var payload = new { identificacaoRequisicao = req.Value, valor = valor.Value, tipo = "D" };
        using var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var resp = await _http.PostAsync("movimentacoes", content, ct);
        if (!resp.IsSuccessStatusCode) await ThrowFor(resp, "Falha ao debitar conta de origem.");
    }

    public async Task CreditarAsync(RequisicaoId req, ContaNumero contaDestino, Dinheiro valor, CancellationToken ct)
    {
        ForwardBearer();
        var payload = new { identificacaoRequisicao = req.Value, numeroConta = contaDestino.Value, valor = valor.Value, tipo = "C" };
        using var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var resp = await _http.PostAsync("movimentacoes", content, ct);
        if (!resp.IsSuccessStatusCode) await ThrowFor(resp, "Falha ao creditar conta de destino.");
    }

    public async Task DebitarMinhaContaAsync(RequisicaoId req, Dinheiro valor, CancellationToken ct)
    {
        ForwardBearer();
        var payload = new { identificacaoRequisicao = req.Value, valor = valor.Value, tipo = "D" };
        using var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var resp = await _http.PostAsync("movimentacoes", content, ct);
        if (!resp.IsSuccessStatusCode) await ThrowFor(resp, "Falha ao debitar conta de origem no estorno.");
    }

    private static async Task ThrowFor(HttpResponseMessage resp, string defaultMessage)
    {
        var body = await resp.Content.ReadAsStringAsync();
        string errorType = "UNKNOWN";
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("type", out var t) && t.ValueKind == JsonValueKind.String)
                errorType = t.GetString()!;
            else if (doc.RootElement.TryGetProperty("code", out var c) && c.ValueKind == JsonValueKind.String)
                errorType = c.GetString()!;
            else if (doc.RootElement.TryGetProperty("error", out var e) && e.ValueKind == JsonValueKind.String)
                errorType = e.GetString()!;
        }
        catch {  }

        throw new ContaCorrenteClientException(
            defaultMessage,
            resp.StatusCode,
            errorType,
            string.IsNullOrWhiteSpace(body) ? null : body);
    }
}
