using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using BankMore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankMore.Infrastructure.Data.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly UsuarioDbContext _db;

    public UsuarioRepository(UsuarioDbContext db)
    {
        _db = db;
    }

    public async Task<Usuario?> ObterPorCpfAsync(string cpf)
    {
        return await _db.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Cpf == cpf);
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        await _db.Usuarios.AddAsync(usuario);
        await _db.SaveChangesAsync();
    }

    public async Task InativarAsync(Guid idUsuario)
    {
        var usuario = await _db.Usuarios.FindAsync(idUsuario);

        if (usuario is null)
            throw new InvalidOperationException("Usuário não encontrado.");

        usuario.Inativar();
        await _db.SaveChangesAsync();
    }

}
