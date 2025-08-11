using BankMore.Application.DTOs;
using MediatR;

namespace BankMore.Application.Queries.ObterTarifasPorConta
{
    public class ObterTarifasPorContaQuery : IRequest<List<TarifaDto>>
    {
        public Guid IdContaCorrente { get; set; }

        public ObterTarifasPorContaQuery(Guid idContaCorrente)
        {
            IdContaCorrente = idContaCorrente;
        }
    }
}
