using Application.Common.Abstractions;
using Application.Common.Abstractions.Caching;
using Application.Common.Abstractions.Identity;
using Domain.Constants;
using Infrastructure.Caching;
using Infrastructure.Identity;
using Infrastructure.Identity.OptionsSetup;
using Infrastructure.Identity.Permissions;
using Infrastructure.Identity.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddPersistence(services, "", "");
        AddRedis(services, "");
        AddIdentity(services);
        AddAuthenticationAuthorization(services);
        AddCaching(services);
        return services;
    }

    private static void AddPersistence(IServiceCollection services, string appDbConString, string identityDbConString)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());

            options.UseSqlServer(appDbConString);
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IdentityContextInitialiser>();

        services.AddDbContext<IdentityContext>(op => op.UseSqlServer(identityDbConString));
    }

    private static void AddRedis(IServiceCollection services, string redisConString)
    {
        services.AddSingleton(ConnectionMultiplexer.Connect(redisConString));
        services.AddStackExchangeRedisCache(op => op.Configuration = redisConString);
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddTransient<IIdentityRoleService, IdentityRoleService>();

        services.AddTransient<IAuthService, AuthService>();

        services.AddTransient<ITokenProviderService, TokenProviderService>();
    }
    private static void AddAuthenticationAuthorization(IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddAuthorizationBuilder();

        services.AddSingleton(TimeProvider.System);

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator));
        });

        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddScoped<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
    }

    private static void AddCaching(IServiceCollection services)
    {
        services.AddLazyCache();
        services.ConfigureOptions<CacheOptionsSetup>();
        services.AddSingleton<IInMemoryCacheService, InMemoryCacheService>();
        services.AddSingleton<IDistributedCacheService, DistributedCacheService>();
    }
}
