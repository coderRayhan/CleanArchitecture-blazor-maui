using Application.Common.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Infrastructure.Caching;
internal sealed class DistributedCacheService(
    IDistributedCache distributedCache,
    IOptions<CacheOptions> cacheOptions,
    ConnectionMultiplexer connectionMultiplexer)
    : IDistributedCacheService
{
    private readonly CacheOptions _cacheOptions = cacheOptions.Value;
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        string? cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);

        return cachedValue is null 
            ? default
            : JsonSerializer.Deserialize<T>(cachedValue);
    }

    public Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
    {
        return await distributedCache.GetStringAsync(key, cancellationToken);
    }

    public void Refresh(string key)
    {
        throw new NotImplementedException();
    }

    public Task RefreshAsync(string key, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out bool _);
    }

    public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = default, CancellationToken cancellationToken = default)
    {
        await distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), GetOptions(slidingExpiration), cancellationToken);

        CacheKeys.TryAdd(key, false);
    }

    public async Task SetStringAsync(string key, string value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        await distributedCache.SetStringAsync(key, value, GetOptions(slidingExpiration), cancellationToken);
        CacheKeys.TryAdd(key, false);
    }

    private DistributedCacheEntryOptions GetOptions(TimeSpan? slidingExpiration)
    {
        var options = new DistributedCacheEntryOptions();
        return slidingExpiration.HasValue
            ? options.SetSlidingExpiration(slidingExpiration.Value)
            : options.SetSlidingExpiration(TimeSpan.FromMinutes(_cacheOptions.SlidingExpiration));
    }
}
