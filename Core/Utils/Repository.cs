using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Utils
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _set;
        public Repository(DbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? where = null) => await _set.AnyAsync(where ?? (x => true));

        public async Task<int> CountAsync(Expression<Func<T, bool>>? where = null) => await _set.CountAsync(where ?? (x => true));

        public async Task DeleteAsync(T entity) => await Task.Run(() => _set.Remove(entity));

        public async Task DeleteAsync(object id)
        {
            var entity = await FindByIdAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task DeleteManyAsync(IEnumerable<T> entities) => await Task.Run(() => _set.RemoveRange(entities));

        public async Task DeleteManyAsync(Expression<Func<T, bool>>? where = null)
        {
            var entities = await FindManyAsync(where);
            await DeleteManyAsync(entities);
        }

        public async Task<T?> FindByIdAsync(object id) => await _set.FindAsync(id);

        public async Task<IEnumerable<T>> FindManyAsync(Expression<Func<T, bool>>? where = null, params string[] includes)
        {
            var data = _set.Where(where ?? (x => true));
            foreach (var include in includes)
            {
                data = data.Include(include);
            }
            return await data.ToListAsync();
        }

        public async Task InsertManyAsync(IEnumerable<T> entities) => await _set.AddRangeAsync(entities);

        public async Task InsertOneAsync(T entity) => await _set.AddAsync(entity);

        public async Task UpdateManyAsync(IEnumerable<T> entities) => await Task.Run(() => _set.UpdateRange(entities));

        public async Task UpdateOneAsync(T entity) => await Task.Run(() => _set.Update(entity));
    }
}
