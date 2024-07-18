using Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

            cfg.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));

            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}
