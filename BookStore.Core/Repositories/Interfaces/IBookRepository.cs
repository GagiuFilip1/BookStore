using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Common;
using BookStore.Core.Models.Entities;
using BookStore.Core.Models.Filters;

namespace BookStore.Core.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task CreateBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(Book book);
        Task<Tuple<int, List<Book>>> SearchBookAsync(Pagination pagination, Ordering ordering, BookFilter filter);
    }
}