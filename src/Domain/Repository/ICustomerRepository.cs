using ELibrary_BorrowingService.Domain.Entity;

namespace ELibrary_BorrowingService.Domain.Repository;
public interface ICustomerRepository : IEntityRepository<Customer, string>
{
}
