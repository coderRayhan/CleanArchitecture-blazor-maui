using Application.Features.Idenity.Models;
using Domain.Shared;

namespace Application.Common.Abstractions.Identity;
public interface IAuthService
{
    Task<Result<AuthenticatedResponse>> Login(string username, string password, CancellationToken cancellationToken = default);

    Task<Result<AuthenticatedResponse>> RefreshToken(string accessToken, string refreshToken, CancellationToken cancellationToken = default);

    Task<(Result Result, string UserId)> ForgotPassword(string email);

    Task<(Result Result, string UserId)> ResetPassword(string email);
}
