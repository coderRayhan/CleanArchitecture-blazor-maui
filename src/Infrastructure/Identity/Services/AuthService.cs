using Application.Common.Abstractions.Identity;
using Application.Common.Constants;
using Application.Features.Identity.Models;
using Domain.Shared;
using Infrastructure.Identity.Model;
using Infrastructure.Identity.OptionsSetup;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Identity.Services;
internal sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    ITokenProviderService tokenProvider,
    IdentityContext context,
    IOptionsSnapshot<JwtOptions> jwtOptions) : IAuthService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    public async Task<Result<AuthenticatedResponse>> Login(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByNameAsync(username)
            ?? await userManager.FindByEmailAsync(username);

        if (user is null)
            return Result.Failure<AuthenticatedResponse>(Error.NotFound(nameof(user), ErrorMessages.USER_NOT_FOUND));

        var result = await userManager.CheckPasswordAsync(user, password);

        if(!result)
            return Result.Failure<AuthenticatedResponse>(Error.NotFound(nameof(user), ErrorMessages.WRONG_USERNAME_PASSWORD));

        var (accessToken, expiresInMinute) = await tokenProvider.GenerateAccessTokenAsync(user.Id);

        var authResponse = new AuthenticatedResponse
        {
            AccessToken = accessToken,
            ExpiresInMinutes = expiresInMinute
        };

        var lastRefreshToken = await context.RefreshTokens.FirstOrDefaultAsync(x => x.ApplicationUserId == user.Id, cancellationToken);

        if(lastRefreshToken is { IsActive: true })
        {
            authResponse.RefreshToken = lastRefreshToken.Token;
            authResponse.RefreshTokenExpiresOn = lastRefreshToken.Expires;

            return Result.Success(authResponse);
        }

        var refreshToken = new RefreshToken
        {
            Token = tokenProvider.GenerateRefreshToken(),
            Expires = DateTime.Now.AddDays(_jwtOptions.RefreshTokenExpires),
            ApplicationUserId = user.Id
        };

        authResponse.RefreshToken = refreshToken.Token;
        authResponse.RefreshTokenExpiresOn = refreshToken.Expires;

        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync(cancellationToken);

        return !string.IsNullOrEmpty(accessToken)
            ? Result.Success(authResponse)
            : Result.Failure<AuthenticatedResponse>(Error.NotFound(nameof(user), ErrorMessages.WRONG_USERNAME_PASSWORD));
    }
    public Task<(Result Result, string UserId)> ForgotPassword(string email)
    {
        throw new NotImplementedException();
    }


    public Task<Result<AuthenticatedResponse>> RefreshToken(string accessToken, string refreshToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<(Result Result, string UserId)> ResetPassword(string email)
    {
        throw new NotImplementedException();
    }
}
