using Application.Common.Abstractions.Caching;
using Application.Common.Abstractions.Identity;
using Domain.Constants;
using Infrastructure.Caching;
using Infrastructure.Identity.OptionsSetup;
using Infrastructure.Identity.Permissions;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddIdentity(services);
        AddAuthenticationAuthorization(services);
        AddCaching(services);
        return services;
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
