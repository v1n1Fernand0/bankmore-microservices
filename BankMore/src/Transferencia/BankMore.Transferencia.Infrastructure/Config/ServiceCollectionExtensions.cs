using System;
using BankMore.Transferencia.Domain.Abstractions;
using BankMore.Transferencia.Infrastructure.Config.Options;
using BankMore.Transferencia.Infrastructure.HttpClients;
using BankMore.Transferencia.Infrastructure.Persistence.EFCore;
using BankMore.Transferencia.Infrastructure.Persistence.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankMore.Transferencia.Infrastructure.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTransferenciaInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        SQLitePCL.Batteries.Init();
        services.AddDbContextPool<TransferenciaDbContext>(opt =>
            opt.UseSqlite(config.GetConnectionString("Default")));

        services.AddScoped<ITransferenciaRepository, TransferenciaRepository>();
        services.AddScoped<IIdempotenciaStore, IdempotenciaStore>();

        services.Configure<ContaCorrenteOptions>(config.GetSection(ContaCorrenteOptions.SectionName));

        services.AddHttpClient<IContaCorrenteClient, ContaCorrenteClient>((sp, http) =>
        {
            var opts = sp
                .GetRequiredService<Microsoft.Extensions.Options.IOptions<ContaCorrenteOptions>>()
                .Value;

            if (string.IsNullOrWhiteSpace(opts.BaseUrl))
                throw new InvalidOperationException("ContaCorrente:BaseUrl não configurado no appsettings.");

            http.BaseAddress = new Uri(opts.BaseUrl, UriKind.Absolute);
            http.Timeout = TimeSpan.FromSeconds(15);
            http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        });

        return services;
    }
}
