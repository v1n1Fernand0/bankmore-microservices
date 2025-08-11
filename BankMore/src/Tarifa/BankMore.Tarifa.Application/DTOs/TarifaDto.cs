namespace BankMore.Application.DTOs
{
    public class TarifaDto
    {
        public Guid IdTarifa { get; set; }
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataMovimento { get; set; }
    }

}
