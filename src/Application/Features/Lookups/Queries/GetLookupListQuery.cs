using Application.Common.Abstractions;
using Application.Common.Abstractions.Contracts;
using Application.Common.Models;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Application.Features.Lookups.Queries;
public sealed record GetLookupListQuery : DataGridModel, ICacheableQuery<PaginatedList<LookupResponse>>
{
    [JsonIgnore]
    public string CacheKey => $"Lookup_{Offset}_{PageSize}";
    [JsonIgnore]
    public TimeSpan? Expiration { get; set; } = null;

    public bool? AllowCache { get; set; } = true;
}

//internal sealed class GetLookupListQueryHandler(
//    IApplicationDbContext dbContext)
//    : IQueryHandler<GetLookupListQuery, PaginatedList<LookupResponse>>
//{
//    public async Task<Result<PaginatedList<LookupResponse>>> Handle(GetLookupListQuery request, CancellationToken cancellationToken)
//    {
//        var list = await dbContext.Lookups.AsQueryable();
//    }
//}
