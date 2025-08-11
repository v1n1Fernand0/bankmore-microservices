namespace BankMore.Application.Events
{
    public class ContaCorrenteCriadaEvent
    {
        public string NumeroConta { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid IdConta { get; set; }
    }
}
