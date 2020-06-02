using System;
using System.Linq;
using BookStore.Core.Common.Requests.Commons;
using BookStore.Core.Enums;
using BookStore.Core.Extensions;
using BookStore.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Core.Models.Filters
{
    public class BookFilter : IFilter<Book>
    {
        public string SearchTerm { get; set; }

        public IQueryable<Book> Filter(IQueryable<Book> filterQuery)
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Enum.TryParse(SearchTerm, out BookState state);
                filterQuery = Guid.TryParse(SearchTerm, out var guid)
                    ? filterQuery.Where(p => p.Id.Equals(guid) || p.UserId.Equals(guid))
                    : filterQuery.Where(p => p.State == state);
            }

            return filterQuery;
        }
    }
}