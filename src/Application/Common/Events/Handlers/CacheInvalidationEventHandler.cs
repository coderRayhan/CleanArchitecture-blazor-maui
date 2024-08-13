using Application.Common.Abstractions.Caching;
using MediatR;

namespace Application.Common.Events.Handlers
{
    internal class CacheInvalidationEventHandler(IDistributedCacheService cacheService)
        : INotificationHandler<CacheInvalidationEvent>
    {
        public async Task Handle(CacheInvalidationEvent notification, CancellationToken cancellationToken)
        {
            await cacheService.RemoveByPrefixAsync(notification.CacheKey, cancellationToken);
        }
    }
}
