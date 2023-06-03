using ELibrary_BorrowingService.Application.Command.Model;

namespace ELibrary_BorrowingService.Application.Command;
public interface IBookingProvider
{
    Task Book(int bookId, string customerId);
    Task UnBook(int bookId, string customerId);
}