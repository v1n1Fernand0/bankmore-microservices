using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BankMore.Transferencia.Infrastructure.HttpClients;

namespace BankMore.Transferencia.API.Middlewares;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (ValidationException vex)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            ctx.Response.ContentType = "application/problem+json";

            var pd = new ProblemDetails
            {
                Title = "VALIDATION_ERROR",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Um ou mais campos são inválidos."
            };
            pd.Extensions["errors"] = vex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            await ctx.Response.WriteAsync(JsonSerializer.Serialize(pd));
        }
        catch (ContaCorrenteClientException cex) when (cex.StatusCode == HttpStatusCode.BadRequest)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            ctx.Response.ContentType = "application/problem+json";

            var pd = new ProblemDetails
            {
                Title = cex.ErrorType ?? "INVALID_REQUEST",
                Status = StatusCodes.Status400BadRequest,
                Detail = cex.Message
            };
            if (!string.IsNullOrWhiteSpace(cex.ResponseBody))
                pd.Extensions["upstream"] = cex.ResponseBody;

            await ctx.Response.WriteAsync(JsonSerializer.Serialize(pd));
        }
        catch (Exception)
        {
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            ctx.Response.ContentType = "application/problem+json";
            var pd = new ProblemDetails
            {
                Title = "INTERNAL_ERROR",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "Ocorreu um erro inesperado."
            };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(pd));
        }
    }
}
