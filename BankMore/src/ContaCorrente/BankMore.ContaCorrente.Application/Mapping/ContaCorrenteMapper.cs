using BankMore.Application.Dtos;
using BankMore.Domain.Entities;

namespace BankMore.Application.Mapping;

public static class ContaCorrenteMapper
{
    public static ContaCorrenteDto ToDto(this ContaCorrente c) =>
        new(c.IdContaCorrente, c.Numero, c.Nome, c.Ativo, c.Saldo);
}
