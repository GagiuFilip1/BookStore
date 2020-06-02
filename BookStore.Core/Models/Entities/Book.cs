using System;
using BookStore.Core.Common;
using BookStore.Core.Enums;

namespace BookStore.Core.Models.Entities
{
    public class Book : IIdentifier
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public BookState State { get; set; }
        public Guid UserId { get; set; } = Guid.Parse("d8edbe41-edcd-42f1-8762-102e0744a15f");
        public User User { get; set; }
    }
}