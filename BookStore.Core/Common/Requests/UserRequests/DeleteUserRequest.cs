using System;

namespace BookStore.Core.Common.Requests.UserRequests
{
    public class DeleteUserRequest
    { 
        public Guid UserId { get; set; }
    }
}