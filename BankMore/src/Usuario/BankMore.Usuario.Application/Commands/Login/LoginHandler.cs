using BankMore.Application.Abstractions;
using BankMore.Application.Common;
using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankMore.Application.Commands.Login;

public sealed class LoginHandler : IRequestHandler<LoginCommand, Result<string>>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IUsuarioCache _cache;
    private readonly IConfiguration _config;

    public LoginHandler(
        IUsuarioRepository usuarios,
        IUsuarioCache cache,
        IConfiguration config)
    {
        _usuarios = usuarios;
        _cache = cache;
        _config = config;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken ct)
    {
        string? cpf = null;

        if (Usuario.CpfEhValido(request.Identificador))
        {
            cpf = request.Identificador;
        }
        else
        {
            var conta = await _cache.ObterContaAsyncPorNumero(request.Identificador);
            if (conta is null)
                return Result<string>.Fail("Conta não encontrada.");

            cpf = conta.Value.Cpf;
        }

        if (cpf is null)
            return Result<string>.Fail("Identificador inválido.");

        var usuario = await _usuarios.ObterPorCpfAsync(cpf);
        if (usuario is null || !usuario.ValidarSenha(request.Senha))
            return Result<string>.Fail("Credenciais inválidas.");

        var token = GerarJwt(usuario);
        return Result<string>.Success(token);
    }

    private string GerarJwt(Usuario usuario)
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("cpf", usuario.Cpf),
            new Claim("sub", usuario.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
