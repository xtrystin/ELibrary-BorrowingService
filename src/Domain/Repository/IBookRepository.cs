using ELibrary_BorrowingService.Domain.Entity;

namespace ELibrary_BorrowingService.Domain.Repository;
public interface IBookRepository : IEntityRepository<Book, int>
{
}
