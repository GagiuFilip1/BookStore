using System;

namespace BookStore.Core.Models.Entities
{
    public class Librarian : User
    {
        public DateTime LastLogged { get; set; }
        public string Email { get; set; }
    }
}