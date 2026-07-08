using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Core.Utils
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }

        private Hashtable? repositories;
        public IRepository<T> Repository<T>() where T : class
        {
            repositories ??= [];

            var type = typeof(T).Name;
            if (!repositories.ContainsKey(type))
            {
                var repoType = typeof(Repository<>);
                var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(T)), context);
                repositories.Add(type, repoInstance);
            }
            return (IRepository<T>)repositories[type]!;
        }
    }
}
