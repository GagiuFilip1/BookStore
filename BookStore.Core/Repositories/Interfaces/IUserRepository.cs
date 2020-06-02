using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Common;
using BookStore.Core.Models.Entities;
using BookStore.Core.Models.Filters;

namespace BookStore.Core.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<Tuple<int, List<User>>> SearchUserAsync(Pagination pagination, Ordering ordering, UserFilter filter);
    }
}