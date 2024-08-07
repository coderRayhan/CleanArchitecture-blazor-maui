﻿namespace Application.Common.Abstractions.Caching;
public interface IDistributedCacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default);

    Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);

    Task SetStringAsync(string key, string value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);

    void Refresh(string key);

    Task RefreshAsync(string key, CancellationToken cancellationToken = default);

    Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken= default);
}