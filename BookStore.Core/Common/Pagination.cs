﻿namespace BookStore.Core.Common
{
    public class Pagination
    {
        public int Take { get; set; } = 100;

        public int Offset { get; set; }      
    }
}