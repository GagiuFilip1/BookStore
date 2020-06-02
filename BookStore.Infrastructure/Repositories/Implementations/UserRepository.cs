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
    public class UserRepository : IUserRepository
    {
        private readonly DataContext m_dataContext;

        public UserRepository(DataContext dataContext)
        {
            m_dataContext = dataContext;
        }

        public async Task CreateUserAsync(User user)
        {
            await m_dataContext.AddAsync(user);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            m_dataContext.Update(user);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            m_dataContext.Remove(user);
            await m_dataContext.SaveChangesAsync();
        }

        public async Task<Tuple<int, List<User>>> SearchUserAsync(Pagination pagination, Ordering ordering, UserFilter filter)
        {
            if (filter == null)
                filter = new UserFilter();
            
            return await filter.Filter(m_dataContext.Users.AsQueryable()) // IQueryable<User>
                .WithOrdering(ordering, null)
                .WithPaginationAsync(pagination);
        }
    }
}