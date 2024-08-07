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
    public async Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
    {
        return await distributedCache.GetStringAsync(key, cancellationToken);
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
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out bool _);
    }

    public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        //check if the cache keys exist in the concurrent dictionary
        var keys = CacheKeys.Keys.Where(k => string.Equals(prefix, PrefixValue(k)));

        //if keys are not found in the concurrent dictionary, fetch them from Redis
        if(!keys.Any())
        {
            //Fetch the keys from redis using the specified prefix pattern
            keys = GetKeysFromRedis(prefix);
        }

        // remove cache asynchronously
        var tasks = keys.Select(k => RemoveAsync(k, cancellationToken));
        
        return Task.WhenAll(tasks);
    }

    public void Refresh(string key)
    {
        distributedCache.Refresh(key);
    }

    public async Task RefreshAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RefreshAsync(key, cancellationToken);
    }

    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        T? cachedValue = await GetAsync<T>(key, cancellationToken);

        if(cachedValue is not null)
            return cachedValue;

        cachedValue = await factory();

        await SetAsync(key, cachedValue, slidingExpiration, cancellationToken);

        return cachedValue;
    }


    private DistributedCacheEntryOptions GetOptions(TimeSpan? slidingExpiration)
    {
        var options = new DistributedCacheEntryOptions();
        return slidingExpiration.HasValue
            ? options.SetSlidingExpiration(slidingExpiration.Value)
            : options.SetSlidingExpiration(TimeSpan.FromMinutes(_cacheOptions.SlidingExpiration));
    }

    private static string PrefixValue(string input, char delimiter = '_')
    {
        string[] parts = input.Split(delimiter);

        return parts.Length > 0 ? parts[0] : input;
    }

    private IEnumerable<string> GetKeysFromRedis(string prefixKey)
    {
        var server = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().First());

        var keys = server.Keys(pattern: $"{prefixKey}");

        return keys.Select(k => k.ToString());
    }

}
