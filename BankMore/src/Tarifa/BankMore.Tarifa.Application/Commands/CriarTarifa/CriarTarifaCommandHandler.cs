using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using MediatR;

namespace BankMore.Application.Commands
{
    public class CriarTarifaCommandHandler : IRequestHandler<CriarTarifaCommand, Guid>
    {
        private readonly ITarifaRepository _repository;

        public CriarTarifaCommandHandler(ITarifaRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CriarTarifaCommand request, CancellationToken cancellationToken)
        {
            var tarifa = new Tarifa(
                request.IdContaCorrente,
                DateTime.Now,
                request.Valor,
                request.Descricao
            );

            await _repository.AdicionarAsync(tarifa, cancellationToken);
            return tarifa.IdTarifa;
        }
    }
}
