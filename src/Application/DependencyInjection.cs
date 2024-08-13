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

            cfg.AddOpenBehavior(typeof(AuthorizationBehaviour<,>));

            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));

            //cfg.AddOpenBehavior(typeof(MemoryCachingBehaviour<,>));

            cfg.AddOpenBehavior(typeof(QueryCachingBehaviour<,>));

            cfg.AddOpenBehavior(typeof(CacheInvalidationBehaviour<,>));

        });

        return services;
    }
}
