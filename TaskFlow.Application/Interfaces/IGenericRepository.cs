using System.Linq.Expressions;

namespace TaskFlow.Application.Interfaces;

public interface IGenericRepository<T>
    where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate);

    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task SaveChangesAsync();
}