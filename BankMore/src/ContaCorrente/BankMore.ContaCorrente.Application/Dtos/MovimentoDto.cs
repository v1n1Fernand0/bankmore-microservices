namespace BankMore.Application.Dtos;

public sealed record MovimentoDto(
    Guid IdMovimento,
    DateTime Data,
    string Tipo,
    decimal Valor
);
