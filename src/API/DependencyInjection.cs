using API.Infrastructure;
using API.Services;
using Application.Common.Abstractions.Identity;

namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        AddScopedServices(services);
        AddExceptionHandlers(services);

        return services;
    }

    private static void AddScopedServices(IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();
    }

    private static void AddExceptionHandlers(IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}
