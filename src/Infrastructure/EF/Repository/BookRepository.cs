﻿using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ELibrary_BorrowingService.Infrastructure.EF.Repository;
public class BookRepository : EntityRepository<Book, int>, IBookRepository
{
    public BookRepository(BookDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Book?> GetAsync(int id)
        => await _dbContext.Books.Include(x => x.BorrowingHistory).ThenInclude(x => x.Customer)
            .Include(x => x.BookingHistory).ThenInclude(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == id);
}
