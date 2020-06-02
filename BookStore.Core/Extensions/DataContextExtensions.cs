using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Core.Common;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Core.Extensions
{
    public static class DataContextExtensions
    {
        public static async Task<Tuple<int, List<TSource>>> WithPaginationAsync<TSource>(this IQueryable<TSource> source, Pagination paging)
        {
            return new Tuple<int, List<TSource>>(
                await source.CountAsync(),
                await source.Skip(paging.Offset).Take(paging.Take)
                    .AsQueryable()
                    .ToListAsync());
        }

        public static IQueryable<TSource> WithOrdering<TSource>(this IQueryable<TSource> source, Ordering ordering, Ordering defaultOrdering) 
        {
            if (ordering == null || string.IsNullOrEmpty(ordering.OrderBy))
                ordering = defaultOrdering;

            return source.OrderBy(ordering.OrderBy, ordering.OrderDirection);
        }
    }
}