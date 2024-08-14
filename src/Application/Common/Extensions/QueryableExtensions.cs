using Application.Common.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Extensions;
internal static class QueryableExtensions
{
    public static async Task<PaginatedList<TResult>> ProjectOrderedQueryableToPaginatedListAsync<T, TResult>(this IOrderedQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        where T : class
    {
        var count = await query.CountAsync(cancellationToken);
        var data = await query
            .AsNoTracking()
            .Skip((pageNumber -1 ) * pageSize)
            .Take(pageSize)
            .ProjectToType<TResult>()
            .ToListAsync(cancellationToken);

        return new PaginatedList<TResult>(data, count, pageNumber, pageSize);
    }

    public static async Task<PaginatedList<TResult>> ProjectQueryableToPaginatedListAsync<T, TResult>(this IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        where T : class
    {
        var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var data = await query
            .AsNoTracking()
            .Skip((pageNumber -1 ) * pageSize)
            .Take(pageSize)
            .ProjectToType<TResult>()
            .ToListAsync(cancellationToken);

        return new PaginatedList<TResult>(data, count, pageNumber, pageSize);
    }
}
