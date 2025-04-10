using DietManagementSystem.Application.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Extensions;

public static class PagedListExtensions
{
    public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return new PagedList<T>(items, source.Count(), pageNumber, pageSize);
    }

    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<T>(items, await source.CountAsync(), pageNumber, pageSize);
    }
}


