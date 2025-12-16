using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Application.Common
{
    public static class QueryableExtensions
    {
        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> query, int page, int pageSize)
        {
            var totalItems = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
