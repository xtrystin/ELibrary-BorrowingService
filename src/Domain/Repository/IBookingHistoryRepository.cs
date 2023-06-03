using ELibrary_BorrowingService.Domain.Entity;

namespace ELibrary_BorrowingService.Domain.Repository;
public interface IBookingHistoryRepository : IEntityRepository<BookingHistory, int>
{
}
