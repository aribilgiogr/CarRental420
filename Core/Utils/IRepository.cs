using System.Linq.Expressions;

namespace Core.Utils
{
    public interface IRepository<T> where T : class
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>>? where = null);
        Task<int> CountAsync(Expression<Func<T, bool>>? where = null);
        Task InsertOneAsync(T entity);
        Task InsertManyAsync(IEnumerable<T> entities);
        Task<T?> FindByIdAsync(object id);
        Task<IEnumerable<T>> FindManyAsync(Expression<Func<T, bool>>? where = null, params string[] includes);
        Task UpdateOneAsync(T entity);
        Task UpdateManyAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteAsync(object id);
        Task DeleteManyAsync(IEnumerable<T> entities);
        Task DeleteManyAsync(Expression<Func<T, bool>>? where = null);
    }
}
