using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.Idempotence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdempotence(this IServiceCollection services)
    {
        Guard.AgainstNull(services);

        services.AddSingleton<IHandleMessageObserver, HandleMessageObserver>();

        return services;
    }
}