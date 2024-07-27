using Application.Common.Abstractions.Identity;
using Application.Features.Identity.Models;
using Domain.Shared;
using Infrastructure.Identity.Model;
using Infrastructure.Identity.OptionsSetup;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Infrastructure.Identity.Services;
internal sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    ITokenProviderService tokenProvider,
    IdentityContext context,
    IOptionsSnapshot<JwtOptions> jwtOptions) : IAuthService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    public Task<Result<AuthenticatedResponse>> Login(string username, string password, CancellationToken cancellationToken = default)
    {

        throw new NotImplementedException();
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
