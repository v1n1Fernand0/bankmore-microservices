using BankMore.Application.Abstractions;
using BankMore.Application.Commands;
using BankMore.Domain.Interfaces;
using BankMore.Infrastructure.Data.Repositories;
using BankMore.Infrastructure.Messaging;
using BankMore.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Conta Corrente API",
        Version = "v1",
        Description = "Serviço de contas correntes da plataforma BankMore"
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
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var connString = builder.Configuration.GetConnectionString("ContaCorrenteDb")
                 ?? "Data Source=Data/conta.db";

builder.Services.AddDbContext<ContaCorrenteDbContext>(opt =>
{
    opt.UseSqlite(connString);
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CriarContaCommand).Assembly));

builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();
builder.Services.AddSingleton<IContaCorrenteEventPublisher, KafkaContaCorrenteEventPublisher>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

try
{
    var dataPath = Path.Combine(app.Environment.ContentRootPath, "Data");
    Directory.CreateDirectory(dataPath);

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ContaCorrenteDbContext>();
    await db.Database.EnsureCreatedAsync();
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Falha ao inicializar o banco SQLite.");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "Conta Corrente API v1");
        o.RoutePrefix = string.Empty;
    });
    app.UseCors("dev");
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Conta Corrente API rodando!").WithOpenApi();

app.Run();
