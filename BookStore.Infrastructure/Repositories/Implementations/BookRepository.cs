using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Common;
using BookStore.Core.Extensions;
using BookStore.Core.Models.Entities;
using BookStore.Core.Models.Filters;
using BookStore.Core.Repositories.Interfaces;
using BookStore.Infrastructure.Data;
using BookStore.Infrastructure.Ioc;

namespace BookStore.Infrastructure.Repositories.Implementations
{
    [RegistrationKind(Type = RegistrationType.Scoped)]
    public class BookRepository : IBookRepository
    {
        private readonly DataContext m_dataContext;

        public BookRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateBookAsync(Book book)
        {
            await m_dataContext.AddAsync(book);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            m_dataContext.Update(book);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(Book book)
        {
            m_dataContext.Remove(book);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task<Tuple<int, List<Book>>> SearchBookAsync(Pagination pagination, Ordering ordering, BookFilter filter)
        {
            return await filter.Filter(m_dataContext.Books.AsQueryable()) // IQueryable<Book>
                .WithOrdering(ordering, null)
                .WithPaginationAsync(pagination);
        }
    }
}