using ELibrary_BorrowingService.Application.Query.Model;

namespace ELibrary_BorrowingService.Application.Query;
public interface IBookingReadProvider
{
    Task<List<BookingReadModel>> GetAll();
    Task<List<BookingReadModel>> GetByBook(int bookId);
    Task<List<BookingReadModel>> GetByCustomer(string customerId);
}