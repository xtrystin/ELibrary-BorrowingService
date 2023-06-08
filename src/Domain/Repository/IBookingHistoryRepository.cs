using ELibrary_BorrowingService.Domain.Entity;

namespace ELibrary_BorrowingService.Domain.Repository;
public interface IBookingHistoryRepository : IEntityRepository<BookingHistory, int>
{
    Task<List<BookingHistory>> GetWithExceededDate(DateTime date);
    Task UpdateRangeAsync(List<BookingHistory> entities);
}
