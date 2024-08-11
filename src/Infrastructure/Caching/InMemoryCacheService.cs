using Application.Common.Abstractions.Caching;
using LazyCache;

namespace Infrastructure.Caching;
internal class InMemoryCacheService(IAppCache memoryCache) :
    IInMemoryCacheService
{
    public static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);
    private readonly IAppCache _memoryCache = memoryCache;
    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        T? result = await _memoryCache.GetOrAddAsync(
            key,
            async () => await factory(),
            expiration ?? DefaultExpiration);

        return result;
    }

    public Task Remove(string key)
    {
        _memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}
