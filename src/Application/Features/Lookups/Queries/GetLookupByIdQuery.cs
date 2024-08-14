using Application.Common.Abstractions;
using Application.Common.Abstractions.Contracts;
using Application.Common.Constants;
using Domain.Shared;
using Mapster;

namespace Application.Features.Lookups.Queries;
public sealed record GetLookupByIdQuery(Guid? Id)
    : ICacheableQuery<LookupResponse>
{
    public string CacheKey => $"Lookup_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => true;
}

internal sealed class GetLookupByIdQueryHandler(
    IApplicationDbContext context)
    : IQueryHandler<GetLookupByIdQuery, LookupResponse>
{
    public async Task<Result<LookupResponse>> Handle(GetLookupByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await context.Lookups.FindAsync(request.Id, cancellationToken);

        if(data is null)
            return Result.Failure<LookupResponse>(Error.NotFound(nameof(request), ErrorMessages.EntityNotFound));

        return Result.Success(data.Adapt<LookupResponse>());
    }
}
