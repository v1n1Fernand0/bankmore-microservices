using MediatR;
using Microsoft.Extensions.Logging;

namespace BankMore.Transferencia.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _log;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> log) => _log = log;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var name = typeof(TRequest).Name;
        _log.LogInformation("Handling {RequestName}", name);
        try
        {
            var resp = await next();
            _log.LogInformation("Handled {RequestName}", name);
            return resp;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Error handling {RequestName}", name);
            throw;
        }
    }
}
