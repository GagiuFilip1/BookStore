using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Core.Common;
using BookStore.Core.Common.Requests.Commons;
using BookStore.Core.Common.Requests.UserRequests;
using BookStore.Core.Enums;
using BookStore.Core.Models.Entities;
using BookStore.Core.Models.Filters;
using BookStore.Core.Repositories.Interfaces;
using BookStore.Core.Services.Interfaces;
using BookStore.Infrastructure.Ioc;
using Microsoft.Extensions.Logging;

namespace BookStore.Infrastructure.Services.Implementations
{
    [RegistrationKind(Type = RegistrationType.Scoped)]
    public class UserService : IUserService
    {
        private readonly IUserRepository m_userRepository;
        private readonly ILogger<UserService> m_logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            m_userRepository = userRepository;
            m_logger = logger;
        }

        public async Task<Response<User>> LoginUserAsync(LoginRequest loginRequest)
        {
            try
            {
                var (_, users) = await m_userRepository.SearchUserAsync(
                    new Pagination(),
                    new Ordering(),
                    new UserFilter
                    {
                        SearchTerm = loginRequest.Username
                    });
                return new Response<User>
                {
                    Data = users.Any(t => t.Password.Equals(loginRequest.Password)) ? users.First() : default,
                    IsSuccesfull = users.Any(t => t.Password.Equals(loginRequest.Password))
                };
            }
            catch (Exception e)
            {
                m_logger.LogError("Could not complete user login", e);
                return new Response<User>
                {
                    Data = null,
                    IsSuccesfull = false,
                    Message = e.Message
                };
            }
        }

        public async Task<Response<Tuple<int, List<User>>>> SearchUsersAsync(SearchRequest<User, UserFilter> searchRequest)
        {
            try
            {
                return new Response<Tuple<int, List<User>>>
                {
                    Data = await m_userRepository.SearchUserAsync(searchRequest.Pagination, searchRequest.Ordering, searchRequest.Filtering as UserFilter),
                    IsSuccesfull = true
                };
            }
            catch (Exception e)
            {
                m_logger.LogCritical("Could not Search for users", e);
                return new Response<Tuple<int, List<User>>>
                {
                    Data = null,
                    IsSuccesfull = true,
                    Message = e.Message
                };
            }
        }

        public async Task<Response> RegisterUserAsync(RegisterUserRequest registerUserRequest)
        {
            try
            {
                var user = ParseUserInfo(registerUserRequest, (UserType) registerUserRequest.UserType);
                user.Id = Guid.NewGuid();
                await m_userRepository.CreateUserAsync(user);
                return new Response
                {
                    IsSuccesfull = true,
                    Message = user.Id.ToString()
                };
            }
            catch (Exception e)
            {
                m_logger.LogCritical("Could not add new User", e);
                return new Response
                {
                    IsSuccesfull = false,
                    Message = e.Message
                };
            }
        }

        public async Task<Response> DeleteUserAsync(DeleteUserRequest deleteUserRequest)
        {
            try
            {
                var (_, users) = await m_userRepository.SearchUserAsync(new Pagination(), new Ordering(), new UserFilter
                {
                    SearchTerm = deleteUserRequest.UserId.ToString()
                });
                if (users.Count.Equals(0))
                    return new Response
                    {
                        IsSuccesfull = false,
                        Message = "Could not find any user with the specified Id"
                    };
                await m_userRepository.DeleteUserAsync(users.First());
                return new Response
                {
                    IsSuccesfull = true
                };
            }
            catch (Exception e)
            {
                m_logger.LogCritical("Could not delete user", e);
                return new Response
                {
                    IsSuccesfull = false,
                    Message = e.Message
                };
            }
        }

        private static User ParseUserInfo(RegisterUserRequest registerUserRequest, UserType type)
        {
            User user;
            if (type.Equals(UserType.Subscriber))
                user = new Subscriber
                {
                    Address = registerUserRequest.Address,
                    Cnp = registerUserRequest.Cnp,
                    Name = registerUserRequest.Name,
                    Password = registerUserRequest.Password,
                    PhoneNumber = registerUserRequest.PhoneNumber,
                    Type = type,
                    Username = registerUserRequest.Username
                };
            else
            {
                user = new Librarian
                {
                    Name = registerUserRequest.Name,
                    Password = registerUserRequest.Password,
                    Type = type,
                    Username = registerUserRequest.Username,
                    Email = registerUserRequest.Email
                };
            }

            return user;
        }
    }
}