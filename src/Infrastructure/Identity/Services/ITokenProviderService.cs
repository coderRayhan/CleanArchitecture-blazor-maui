namespace Infrastructure.Identity.Services;
internal interface ITokenProviderService
{
    Task<(string AccessToken, int ExpiresInMinutes)> GenerateAccessTokenAsync(string userId);

    string GenerateRefreshToken();
}
