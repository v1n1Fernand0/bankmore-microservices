namespace BankMore.Domain.Messages;

public static class DomainMessages
{
    public static class ContaCorrenteMensagens
    {
        public const string NomeInvalido = "Nome não pode ser vazio.";
        public const string ContaInativa = "Conta inativa não pode registrar movimento.";
        public const string ValorInvalido = "Valor do movimento deve ser maior que zero.";
        public const string SaldoInsuficiente = "Saldo insuficiente para o débito.";
        public const string SenhaOuSaltInvalido = "Senha ou salt inválidos.";
    }

    public static class MovimentoMensagens
    {
        public const string IdInvalido = "O identificador do movimento é obrigatório.";
        public const string ContaObrigatoria = "A conta corrente associada é obrigatória.";
        public const string DataObrigatoria = "A data do movimento é obrigatória.";
        public const string TipoInvalido = "Tipo de movimento inválido.";
        public const string ValorInvalido = "O valor do movimento deve ser maior que zero.";
    }

    public static class IdempotenciaMensagens
    {
        public const string ChaveInvalida = "Chave de idempotência inválida.";
        public const string RequisicaoVazia = "Requisição não pode ser vazia.";
        public const string ResultadoVazio = "Resultado não pode ser vazio.";
    }
}
