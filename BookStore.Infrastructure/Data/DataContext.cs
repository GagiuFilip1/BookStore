using BookStore.Core.Enums;
using BookStore.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Data
{
    public sealed class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Book> Books { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetDescriminators(modelBuilder);
        }

        private static void SetDescriminators(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasDiscriminator<UserType>("Type")
                .HasValue<Librarian>(UserType.Librarian)
                .HasValue<Subscriber>(UserType.Subscriber);
        }

        public static void CreateForeignKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(t => t.User)
                .WithMany(t => t.Books);
        }
    }
}