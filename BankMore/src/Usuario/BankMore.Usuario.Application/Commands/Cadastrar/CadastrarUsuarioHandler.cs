using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using BankMore.Application.Common;
using MediatR;

namespace BankMore.Application.Commands;

public sealed class CadastrarUsuarioHandler : IRequestHandler<CadastrarUsuarioCommand, Result<Guid>>
{
    private readonly IUsuarioRepository _usuarios;

    public CadastrarUsuarioHandler(IUsuarioRepository usuarios)
    {
        _usuarios = usuarios;
    }

    public async Task<Result<Guid>> Handle(CadastrarUsuarioCommand request, CancellationToken ct)
    {
        try
        {
            var usuario = new Usuario(request.Cpf, request.Senha);
            await _usuarios.AdicionarAsync(usuario);
            return Result<Guid>.Success(usuario.Id);
        }
        catch (ArgumentException ex)
        {
            return Result<Guid>.Fail(ex.Message); 
        }
    }
}
