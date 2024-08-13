using Application.Common.Abstractions.Caching;
using Application.Common.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;
internal sealed class CacheInvalidationBehaviour<TRequest, TResponse>(
    ILogger<CacheInvalidationBehaviour<TRequest, TResponse>> logger,
    IDistributedCacheService cacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheInvalidatorCommand
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next().ConfigureAwait(false);

        if (!string.IsNullOrEmpty(request.CacheKey))
        {
            await cacheService.RemoveByPrefixAsync(request.CacheKey, CancellationToken.None);

            logger.LogInformation("Cache value of {CacheKey} expired with {@Request}", request.CacheKey, request);
        }

        return result;
    }
}
