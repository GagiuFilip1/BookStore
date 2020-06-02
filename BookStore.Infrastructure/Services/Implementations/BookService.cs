using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Core.Common;
using BookStore.Core.Common.Requests.Commons;

using BookStore.Core.Models.Entities;
using BookStore.Core.Models.Filters;
using BookStore.Core.Repositories.Interfaces;
using BookStore.Core.Services.Interfaces;
using BookStore.Infrastructure.Ioc;
using Microsoft.Extensions.Logging;

namespace BookStore.Infrastructure.Services.Implementations
{
    [RegistrationKind(Type = RegistrationType.Scoped)]
    public class BookService : IBookService
    {
        private readonly IBookRepository m_bookRepository;
        private readonly ILogger<BookService> m_logger;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
        {
            m_bookRepository = bookRepository;
            m_logger = logger;
        }

        public async Task<Response<Tuple<int, List<Book>>>> SearchBooksAsync(SearchRequest<Book, BookFilter> searchRequest)
        {
            if(searchRequest.Filtering == null)
                searchRequest.Filtering = new BookFilter();
            try
            {
                return new Response<Tuple<int, List<Book>>>
                {
                    Data = await m_bookRepository.SearchBookAsync(searchRequest.Pagination, searchRequest.Ordering, searchRequest.Filtering as BookFilter),
                    IsSuccesfull = true
                };
            }
            catch (Exception e)
            {
                m_logger.LogCritical("Could not Search for Books", e);
                return new Response<Tuple<int, List<Book>>>
                {
                    Data = null,
                    IsSuccesfull = true,
                    Message = e.Message
                };
            }
        }

        public async Task<Response> RegisterBookAsync(Book book)
        {
            try
            {
                book.Id = Guid.NewGuid();
                await m_bookRepository.CreateBookAsync(book);
                return new Response
                {
                    IsSuccesfull = true,
                    Message = book.Id.ToString()
                };
            }
            catch (Exception e)
            {
                m_logger.LogCritical("Could not add new Book", e);
                return new Response
                {
                    IsSuccesfull = false,
                    Message = e.Message
                };
            }
        }

        public async Task<Response> UpdateBookAsync(Book book)
        {
            try
            {
                await m_bookRepository.UpdateBookAsync(book);
                return new Response
                {
                    IsSuccesfull = true,
                    Message = book.Id.ToString()
                };
            }
            catch (Exception e)
            {
                m_logger.LogCritical("Could not update new Book", e);
                return new Response
                {
                    IsSuccesfull = false,
                    Message = e.Message
                };
            }
        }

        public async Task<Response> DeleteBookAsync(Guid id)
        {
            try
            {
                var (_, books) = await m_bookRepository.SearchBookAsync(new Pagination(), new Ordering(), new BookFilter
                {
                    SearchTerm = id.ToString()
                });
                if (books.Count.Equals(0))
                    return new Response
                    {
                        IsSuccesfull = false,
                        Message = "Could not find any Book with the specified Id"
                    };
                await m_bookRepository.DeleteBookAsync(books.First());
                return new Response
                {
                    IsSuccesfull = true,
                    Message = books.First().Id.ToString()
                };
            }
            catch (Exception e)
            {
                m_logger.LogCritical("Could not delete Book", e);
                return new Response
                {
                    IsSuccesfull = false,
                    Message = e.Message
                };
            }
        }
    }
}