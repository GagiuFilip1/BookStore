using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Common.Requests.Commons;
using BookStore.Core.Common.Requests.UserRequests;
using BookStore.Core.Models.Entities;
using BookStore.Core.Models.Filters;
using BookStore.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BookStore.Presentation.Controllers.Implementations
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> m_logger;
        private readonly IUserService m_userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            m_logger = logger;
            m_userService = userService;
        }

        [HttpPost]
        [Route("get")]
        public async Task<ActionResult<Tuple<int, List<User>>>> GetUsersAsync([FromBody] SearchRequest<User, UserFilter> request)
        {
            var response = await m_userService.SearchUsersAsync(request);
            return response.Data;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> RegisterUserAsync([FromBody] RegisterUserRequest request)
        {
            var response = await m_userService.RegisterUserAsync(request);
            return Guid.Parse(response.Message);
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteUserAsync([FromBody] string id)
        {
            var deleteUserRequest = new DeleteUserRequest
            {
                UserId = Guid.Parse(id)
            };
            var response = await m_userService.DeleteUserAsync(deleteUserRequest);
            return string.Empty;
        }
    }
}