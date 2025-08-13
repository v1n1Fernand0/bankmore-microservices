using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BankMore.Transferencia.Application.Behaviors;

namespace BankMore.Transferencia.Application.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTransferenciaApplication(
        this IServiceCollection services, IConfiguration _)
    {
        var asm = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(asm));

        services.AddValidatorsFromAssembly(asm);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}
