using BankMore.Application.Abstractions;
using BankMore.Application.Cache;
using BankMore.Application.EventHandlers;
using BankMore.Application.Events;
using BankMore.Domain.Entities;
using BankMore.Domain.Interfaces;
using BankMore.Infrastructure.Data.Repositories;
using BankMore.Infrastructure.Messaging;
using BankMore.Infrastructure.Persistence;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


SQLitePCL.Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UsuarioDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddSingleton<IUsuarioCache, UsuarioCache>();
builder.Services.AddScoped<IContaCorrenteCriadaHandler, ContaCorrenteCriadaHandler>();
builder.Services.AddHostedService<ContaCorrenteCriadaConsumer>();

var jwtConfig = builder.Configuration.GetSection("Jwt");
var secretKey = jwtConfig["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Usuário API",
        Version = "v1",
        Description = "Serviço de usuários da plataforma BankMore"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Ex: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new[] { "Bearer" }
        }
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Usuário API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapGet("/", () => "Usuário API rodando!").WithOpenApi();

app.MapPost("/testar-evento", async (IConfiguration config) =>
{
    var evento = new ContaCorrenteCriadaEvent
    {
        IdConta = Guid.NewGuid(),
        IdUsuario = Guid.NewGuid(),
        NumeroConta = "123456",
    };

    var kafkaConfig = new ProducerConfig
    {
        BootstrapServers = config["Kafka:BootstrapServers"]
    };

    using var producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
    var payload = JsonSerializer.Serialize(evento);

    await producer.ProduceAsync("conta-corrente-criada", new Message<Null, string> { Value = payload });

    return Results.Ok("Evento enviado com sucesso!");
}).WithOpenApi();

app.MapGet("/cache/{idUsuario:guid}", async (Guid idUsuario, IUsuarioCache cache) =>
{
    var resultado = await cache.ObterContaAsync(idUsuario);

    return resultado is not null
        ? Results.Ok(new
        {
            IdConta = resultado.Value.IdConta,
            NumeroConta = resultado.Value.NumeroConta
        })
        : Results.NotFound(new { error = "Usuário não encontrado no cache." });
}).WithOpenApi();

app.MapPost("/usuarios", async (Usuario usuario, IUsuarioRepository repo) =>
{
    var existe = await repo.ObterPorCpfAsync(usuario.Cpf);
    if (existe is not null)
        return Results.BadRequest(new { error = "CPF já cadastrado." });

    await repo.AdicionarAsync(usuario);
    return Results.Created($"/usuarios/{usuario.Id}", new { usuario.Id, usuario.Cpf });
}).WithOpenApi();

app.MapPost("/login", async (IUsuarioRepository repo, IConfiguration config, string cpf, string senha) =>
{
    var usuario = await repo.ObterPorCpfAsync(cpf);
    if (usuario is null || !usuario.ValidarSenha(senha) || !usuario.Ativo)
        return Results.Unauthorized();

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(ClaimTypes.Name, usuario.Cpf)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: config["Jwt:Issuer"],
        audience: config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds);

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new { token = tokenString });
}).WithOpenApi();

app.MapPost("/usuarios/{idUsuario:guid}/inativar", async (Guid idUsuario, IUsuarioRepository repo) =>
{
    await repo.InativarAsync(idUsuario);
    return Results.Ok(new { message = "Usuário inativado com sucesso." });
}).RequireAuthorization().WithOpenApi();

app.Run();
