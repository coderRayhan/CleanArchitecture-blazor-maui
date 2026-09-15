using Application.Common.Abstractions.Idempotency;
using Infrastructure.Caching;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Idempotency;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    private const string DefaultConnection = nameof(DefaultConnection);
    private const string IdentityConnection = nameof(IdentityConnection);
    private const string RedisConnection = nameof(RedisConnection);
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        PersistenceServiceExtension.AddPersistenceService(services, configuration);
        CachingServiceExtension.AddCachingService(services, configuration);
        IdentityServiceExtension.AddIdentityService(services, configuration);

        services.AddScoped<IIdempotencyStore, SqlServerIdempotencyStore>();
        return services;
    }

}