using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BankMore.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddContaCorrenteApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}
