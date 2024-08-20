using API.Infrastructure;
using API.Services;
using Application.Common.Abstractions.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        AddDatabaseDeveloperPageExceptionFilter(services);
        AddJsonConfiguration(services);
        AddScopedServices(services);
        AddExceptionHandlers(services);
        AddHttpContextAccessor(services);
        ConfigureApiBehaviour(services);
        return services;
    }

    private static void AddDatabaseDeveloperPageExceptionFilter(IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
    }

    private static void AddJsonConfiguration(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        });
    }

    private static void AddScopedServices(IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();
    }

    private static void AddHttpContextAccessor(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
    }

    private static void AddExceptionHandlers(IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    private static void ConfigureApiBehaviour(IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        options.SuppressModelStateInvalidFilter = true);
    }

    private static void AddOpenApiDocument(IServiceCollection services)
    {
        services.AddOpenApiDocument((settings, sp) =>
        {
            settings.Title = "API";


        });
    }
}
