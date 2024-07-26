using Domain.Constants;
using Infrastructure.Identity.Model;
using Infrastructure.Identity.Permissions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using PermissionConstant = Application.Common.Security.Permissions;

namespace Infrastructure.Identity;

public static class IdentityInitialiserExtensions
{
    public static async Task IdentityInitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var identityInitialiser = scope.ServiceProvider.GetRequiredService<IdentityContextInitialiser>();

        await identityInitialiser.InitializeAsync();

        await identityInitialiser.SeedAsync();
    }
}
internal sealed class IdentityContextInitialiser(
    ILogger<IdentityContextInitialiser> logger,
    IdentityContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
{
    public async Task InitializeAsync()
    {
        try 
        {
            await context.Database.MigrateAsync();
        } 
        catch (Exception ex) 
        {
            logger.LogError(ex, "An error occured while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await SeedDefaultIdentityAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured while seeding the database.");
            throw;
        }
    }

    public async Task SeedDefaultIdentityAsync()
    {
        //default role
        var administrator = new IdentityRole(Roles.Administrator);

        if(roleManager.Roles.All(e => e.Name != administrator.Name))
        {
            await roleManager.CreateAsync(administrator);
        }

        // get permissions
        var features = PermissionConstant.GetAllNestedModule(typeof(PermissionConstant.Admin));
        features.AddRange(PermissionConstant.GetAllNestedModule(typeof(PermissionConstant.CommonSetup)));

        var permissions = PermissionConstant.GetPermissionsForModules(features);

        //default permissions
        foreach (var permission in permissions)
        {
            await roleManager.AddClaimAsync(administrator, new Claim(CustomClaimTypes.Permission, permission));
        }

        //default users
        var admin = new ApplicationUser
        {
            UserName = "admin@localhost",
            Email = "admin@localhost"
        };

        if(userManager.Users.All(e => e.UserName != admin.UserName))
        {
            await userManager.CreateAsync(admin, "Admin@123");

            if (!string.IsNullOrWhiteSpace(administrator.Name))
            {
                await userManager.AddToRoleAsync(admin, administrator.Name);
            }
        }
    }
}
