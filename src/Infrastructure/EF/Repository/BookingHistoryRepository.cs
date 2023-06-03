using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ELibrary_BorrowingService.Infrastructure.EF.Repository;
public class BookingHistoryRepository : EntityRepository<BookingHistory, int>, IBookingHistoryRepository
{
    private readonly BookDbContext _dbContext;

    public BookingHistoryRepository(BookDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<BookingHistory?> GetAsync(int id)   
        => await _dbContext.BookingHistory.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<BookingHistory>> GetWithExceededDate(DateTime date)
        => await _dbContext.BookingHistory.Where(x => x.IsLimitDateExceeded(date)).ToListAsync();
}
