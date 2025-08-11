namespace BankMore.Application.Events;

public class ContaCorrenteCriadaEvent
{
    public Guid IdConta { get; set; }
    public Guid IdUsuario { get; set; }
    public string NumeroConta { get; set; } = string.Empty;
}
