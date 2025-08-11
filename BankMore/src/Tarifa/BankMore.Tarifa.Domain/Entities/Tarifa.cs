namespace BankMore.Domain.Entities
{
    public class Tarifa
    {
        public Guid IdTarifa { get; private set; }
        public Guid IdContaCorrente { get; private set; }
        public DateTime DataMovimento { get; private set; }
        public decimal Valor { get; private set; }
        public string Descricao { get; private set; } = string.Empty;

        private Tarifa() { }

        public Tarifa(Guid idContaCorrente, DateTime dataMovimento, decimal valor, string descricao)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor da tarifa deve ser positivo.");

            if (dataMovimento.Date > DateTime.Today)
                throw new ArgumentException("Data do movimento não pode ser futura.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição da tarifa é obrigatória.");

            IdTarifa = Guid.NewGuid();
            IdContaCorrente = idContaCorrente;
            DataMovimento = dataMovimento;
            Valor = Math.Round(valor, 2);
            Descricao = descricao;
        }

        public static Tarifa CalcularPorPercentual(Guid idContaCorrente, DateTime data, decimal baseValor, decimal percentual, string descricao)
        {
            var valorTarifa = Math.Round(baseValor * percentual / 100, 2);
            return new Tarifa(idContaCorrente, data, valorTarifa, descricao);
        }
    }
}
