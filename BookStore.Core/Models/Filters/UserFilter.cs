using System;
using System.Linq;
using BookStore.Core.Common.Requests.Commons;
using BookStore.Core.Extensions;
using BookStore.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Core.Models.Filters
{
    public class UserFilter : IFilter<User>
    {
        public string SearchTerm { get; set; }
        
        public IQueryable<User> Filter(IQueryable<User> filterQuery)
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                filterQuery = Guid.TryParse(SearchTerm, out var guid)
                    ? filterQuery.Where(p => p.Id.Equals(guid))
                    : filterQuery.Where(p => EF.Functions.Like(p.Username, SearchTerm.ToMySqlLikeSyntax()));
            }

            return filterQuery;
        }
    }
}