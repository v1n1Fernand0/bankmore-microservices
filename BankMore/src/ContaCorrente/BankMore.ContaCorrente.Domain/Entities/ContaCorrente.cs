using BankMore.Domain.Enums;
using static BankMore.Domain.Messages.DomainMessages;

namespace BankMore.Domain.Entities;

public class ContaCorrente
{
    public Guid IdContaCorrente { get; private set; }
    public Guid IdUsuario { get; private set; }
    public string Numero { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public bool Ativo { get; private set; }

    private readonly List<Movimento> _movimentos = new();
    public IReadOnlyCollection<Movimento> Movimentos => _movimentos.AsReadOnly();

    public decimal Saldo => _movimentos
        .Sum(m => m.TipoMovimento == TipoMovimento.Credito ? m.Valor : -m.Valor);

    protected ContaCorrente() { }

    public ContaCorrente(Guid id, Guid idUsuario, string numero, string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException(ContaCorrenteMensagens.NomeInvalido, nameof(nome));

        IdContaCorrente = id;
        IdUsuario = idUsuario;
        Numero = numero;
        Nome = nome;
        Ativo = true;
    }

    public void AlterarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException(ContaCorrenteMensagens.NomeInvalido, nameof(novoNome));

        Nome = novoNome;
    }

    public void Ativar() => Ativo = true;

    public void Inativar() => Ativo = false;

    public void RegistrarMovimento(decimal valor, TipoMovimento tipo, DateTime data)
    {
        if (!Ativo)
            throw new InvalidOperationException(ContaCorrenteMensagens.ContaInativa);

        if (valor <= 0)
            throw new ArgumentException(ContaCorrenteMensagens.ValorInvalido, nameof(valor));

        if (tipo == TipoMovimento.Debito && valor > Saldo)
            throw new InvalidOperationException(ContaCorrenteMensagens.SaldoInsuficiente);

        var movimento = new Movimento(
            Guid.NewGuid(),
            IdContaCorrente,
            data,
            tipo,
            valor
        );

        _movimentos.Add(movimento);
    }
}
