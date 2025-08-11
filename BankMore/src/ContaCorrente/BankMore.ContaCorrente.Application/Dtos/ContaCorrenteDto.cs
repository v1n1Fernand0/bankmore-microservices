namespace BankMore.Application.Dtos;

public sealed record ContaCorrenteDto(
    Guid IdContaCorrente,
    string Numero,
    string Nome,
    bool Ativo,
    decimal Saldo);
