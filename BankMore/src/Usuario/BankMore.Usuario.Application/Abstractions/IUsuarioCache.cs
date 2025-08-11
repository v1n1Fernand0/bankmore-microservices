namespace BankMore.Application.Abstractions
{
    public interface IUsuarioCache
    {
        Task AtualizarContaAsync(Guid idUsuario, Guid idConta, string numeroConta);
        Task<(Guid IdConta, string NumeroConta)?> ObterContaAsync(Guid idUsuario);
        Task<(string Cpf, Guid IdUsuario)?> ObterContaAsyncPorNumero(string numeroConta);
    }
}
