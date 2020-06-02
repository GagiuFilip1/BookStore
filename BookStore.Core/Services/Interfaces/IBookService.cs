using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Common;
using BookStore.Core.Common.Requests.Commons;
using BookStore.Core.Models.Entities;
using BookStore.Core.Models.Filters;

namespace BookStore.Core.Services.Interfaces
{
    public interface IBookService
    {
        Task<Response<Tuple<int, List<Book>>>> SearchBooksAsync(SearchRequest<Book, BookFilter> searchRequest);
        Task<Response> RegisterBookAsync(Book book);
        Task<Response> UpdateBookAsync(Book book);
        Task<Response> DeleteBookAsync(Guid id);
    }
}