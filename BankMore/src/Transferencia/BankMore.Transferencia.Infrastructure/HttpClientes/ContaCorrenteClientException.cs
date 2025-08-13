using System.Net;

namespace BankMore.Transferencia.Infrastructure.HttpClients;

public sealed class ContaCorrenteClientException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string ErrorType { get; }
    public string? ResponseBody { get; }

    public ContaCorrenteClientException(string message, HttpStatusCode statusCode, string errorType, string? body = null)
        : base(message)
        => (StatusCode, ErrorType, ResponseBody) = (statusCode, errorType, body);
}
