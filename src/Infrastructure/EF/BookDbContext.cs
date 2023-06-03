using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Infrastructure.EF.Config;
using Microsoft.EntityFrameworkCore;

namespace ELibrary_BorrowingService.Infrastructure.EF
{
    public class BookDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "borrowingService";
        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BorrowingHistory> BorrowingHistory { get; set; }
        public DbSet<BookingHistory> BookingHistory { get; set; }

        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);

            modelBuilder.ApplyConfiguration(new BookEntityTypeConfig());
            modelBuilder.ApplyConfiguration(new CustomerTypeConfig());
            modelBuilder.ApplyConfiguration(new BorrowingHistoryTypeConfig());
            modelBuilder.ApplyConfiguration(new BookingHistoryTypeConfig());
        }
    }
}
