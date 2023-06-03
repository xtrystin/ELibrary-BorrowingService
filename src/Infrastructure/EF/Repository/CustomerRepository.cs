using ELibrary_BorrowingService.Domain.Entity;
using ELibrary_BorrowingService.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ELibrary_BorrowingService.Infrastructure.EF.Repository;
public class CustomerRepository : EntityRepository<Customer, string>, ICustomerRepository
{
    private readonly BookDbContext _dbContext;

    public CustomerRepository(BookDbContext dbContext) : base(dbContext)
	{
        _dbContext = dbContext;
    }

    public override async Task<Customer?> GetAsync(string id) 
        => await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
}
