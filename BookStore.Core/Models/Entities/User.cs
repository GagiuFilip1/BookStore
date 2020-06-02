using System;
using System.Collections.Generic;
using BookStore.Core.Common;
using BookStore.Core.Enums;

namespace BookStore.Core.Models.Entities
{
    public abstract class User : IIdentifier
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public UserType Type { get; set; }
        public List<Book> Books { get; set; }
    }
}