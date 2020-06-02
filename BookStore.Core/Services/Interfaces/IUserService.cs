using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Common.Requests.Commons;
using BookStore.Core.Common.Requests.UserRequests;
using BookStore.Core.Models.Entities;
using BookStore.Core.Models.Filters;

namespace BookStore.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<Response<User>> LoginUserAsync(LoginRequest loginRequest);
        Task<Response<Tuple<int, List<User>>>> SearchUsersAsync(SearchRequest<User, UserFilter> searchRequest);
        Task<Response> RegisterUserAsync(RegisterUserRequest registerUserRequest);
        Task<Response> DeleteUserAsync(DeleteUserRequest deleteUserRequest);
    }
}