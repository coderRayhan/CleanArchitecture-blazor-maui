using Application.Common.Abstractions.Caching;
using Application.Common.Abstractions.Contracts;
using Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;
internal sealed class QueryCachingBehaviour<TRequest, TResponse>(
    IDistributedCacheService cacheService,
    ILogger<QueryCachingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableQuery
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(request.AllowCache ?? true)
        {
            TResponse? cachedResult = await cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation($"Cache hit for {typeof(TRequest).FullName}");

                return cachedResult;
            }
        }

        TResponse result = await next();

        if (result.IsSuccess)
        {
            await cacheService.SetAsync(
                request.CacheKey, 
                result, 
                request.Expiration, 
                cancellationToken)
                .ConfigureAwait(false);
        }

        return result;
    }
}
