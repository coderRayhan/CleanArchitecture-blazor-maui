using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Cryptography;

namespace Web.Services.UserPreferences;

public interface IUserPreferenceService
{
    public Task SaveUserPreferences(UserPreferences userPreferences);

    public Task<UserPreferences> LoadUserPreferences();
}

public class UserPreferenceService : IUserPreferenceService
{
    public const string Key = "userPreferences";
    private readonly ProtectedLocalStorage _localStorage;

    public UserPreferenceService(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<UserPreferences> LoadUserPreferences()
    {
        try
        {
            var result = await _localStorage.GetAsync<UserPreferences>(Key);
            if(result.Success && result.Value is not null)
            {
                return result.Value;
            }
            return new UserPreferences();
        }
        catch (CryptographicException)
        {
            await _localStorage.DeleteAsync(Key);
            return new UserPreferences();
            throw;
        }
        catch (Exception)
        {
            return new UserPreferences();
        }
    }

    public async Task SaveUserPreferences(UserPreferences userPreferences)
    {
        await _localStorage.SetAsync(Key, userPreferences);
    }
}
