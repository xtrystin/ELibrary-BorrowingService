using ELibrary_BorrowingService.Application.Command.Model;

namespace ELibrary_BorrowingService.Application.Command;
public interface IBorrowProvider
{
    Task Borrow(int bookId, string customerId);
    Task Return(int bookId, string customerId);
}