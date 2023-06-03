namespace ELibrary_BorrowingService.Domain.Repository;
public interface IEntityRepository<T, U>
{
    Task AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task<T> GetAsync(U id);
    Task UpdateAsync(T entity);
}