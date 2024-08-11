using Application.Common.Abstractions.Caching;
using Application.Common.Abstractions.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;
internal sealed class MemoryCachingBehaviour<TRequest, TResponse>(
    IInMemoryCacheService cacheService,
    ILogger<MemoryCachingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableQuery
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogTrace("{Request} is caching with {@Request}", nameof(request), request);
        
        return await cacheService.GetOrCreateAsync(
            request.CacheKey,
            async () => await next(),
            request.Expiration,
            cancellationToken);
    }
}
