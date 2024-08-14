using Application.Common.Abstractions;
using Application.Common.Abstractions.Contracts;
using Application.Common.Models;
using Domain.Shared;
using System.Text.Json.Serialization;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Application.Common.Extensions;
using Domain.Entities;

namespace Application.Features.Lookups.Queries;
public sealed record GetLookupListQuery : DataGridModel, ICacheableQuery<PaginatedList<LookupResponse>>
{
    [JsonIgnore]
    public string CacheKey => $"Lookup_{Offset}_{PageSize}";
    [JsonIgnore]
    public TimeSpan? Expiration { get; set; } = null;

    public bool? AllowCache { get; set; } = true;
}

internal sealed class GetLookupListQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetLookupListQuery, PaginatedList<LookupResponse>>
{
    public async Task<Result<PaginatedList<LookupResponse>>> Handle(GetLookupListQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Lookups
            .OrderBy($"{request.SortField} {request.SortingDirection}")
            .Where(e => string.IsNullOrEmpty(request.GlobalFilterText) || 
            EF.Functions.Like(e.Name, $"%{request.GlobalFilterText}%") ||
            EF.Functions.Like(e.NameBN, $"%{request.GlobalFilterText}%") ||
            EF.Functions.Like(e.Name, $"%{request.GlobalFilterText}%"))
            .ProjectQueryableToPaginatedListAsync<Lookup, LookupResponse>(
            request.PageNumber, 
            request.PageSize, 
            cancellationToken);
    }
}
