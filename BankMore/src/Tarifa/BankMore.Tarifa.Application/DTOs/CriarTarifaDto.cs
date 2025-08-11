namespace BankMore.Application.DTOs
{
    public class CriarTarifaDto
    {
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }

}
