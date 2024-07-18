using Domain.Shared;

namespace Application.Features.Idenity.Models;
public class AuthenticatedResponse
{
    public required string AccessToken { get; init; }
    public string TokenType { get; } = "Bearer";
    public required int ExpiresInMinutes { get; init; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiresOn { get; set; }
}
