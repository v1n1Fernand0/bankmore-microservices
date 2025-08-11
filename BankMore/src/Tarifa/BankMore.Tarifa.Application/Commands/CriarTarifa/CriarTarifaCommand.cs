using MediatR;

namespace BankMore.Application.Commands
{
    public class CriarTarifaCommand : IRequest<Guid>
    {
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }

}
