using ELibrary_BorrowingService.Domain.Repository;

namespace ELibrary_BorrowingService.Infrastructure.EF.Repository
{
    public abstract class EntityRepository<T, U> : IEntityRepository<T, U>
    {
        protected readonly BookDbContext _dbContext;

        public EntityRepository(BookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public abstract Task<T?> GetAsync(U id);

        public async Task UpdateAsync(T book)
        {
            _dbContext.Update(book);
            await _dbContext.SaveChangesAsync();
        }
    }
}
