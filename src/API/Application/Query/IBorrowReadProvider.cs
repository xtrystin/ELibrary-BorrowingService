using ELibrary_BorrowingService.Application.Query.Model;

namespace ELibrary_BorrowingService.Application.Query;
public interface IBorrowReadProvider
{
    Task<List<BorrowReadModel>> GetAll();
    Task<List<BorrowReadModel>> GetByBook(int bookId);
    Task<List<BorrowReadModel>> GetByCustomer(string customerId);
}