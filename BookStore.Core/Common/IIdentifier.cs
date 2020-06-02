using System;

namespace BookStore.Core.Common
{
    public interface IIdentifier
    {
        Guid Id { get; set; }
    }
}