using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Magalu.Challenge.Data
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        Task<TEntity> FindAsync(params object[] keyValues);

        Task<IPagedList<TEntity>> GetPagedListAsync(int pageIndex, int pageSize);
    }

    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public ReadOnlyRepository(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException();
        }

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await Context.FindAsync<TEntity>(keyValues);
        }

        public Task<IPagedList<TEntity>> GetPagedListAsync(int pageIndex, int pageSize)
        {
            return Context.Set<TEntity>().ToPagedListAsync(pageIndex, pageSize);
        }
    }
}
