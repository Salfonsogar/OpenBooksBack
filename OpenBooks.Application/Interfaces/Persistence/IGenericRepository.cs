using System.Linq.Expressions;

namespace OpenBooks.Application.Interfaces.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task SaveChangesAsync();

        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes
        );

        IQueryable<T> Query(
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[] includes
        );
    }

}
