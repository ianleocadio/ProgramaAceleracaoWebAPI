using Database.Contexts;
using Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Database.Repositories.Common
{
    public abstract class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : Entity<TEntity>, new()
    {
        protected DbContextBase Context { get; }

        protected DbSet<TEntity> DbSet { get => this.Context.Set<TEntity>(); }

        public Repository(DbContextBase context)
        {
            this.Context = context;
        }

        public virtual async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await this.DbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            await this.DbSet.AddRangeAsync(entities, cancellationToken);
        }

        public async Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await this.DbSet.AnyAsync(e => e.ID == id, cancellationToken);
        }

        public virtual async Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await this.DbSet.FirstOrDefaultAsync(e => e.ID == id, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this.DbSet.ToListAsync(cancellationToken);
        }

        public virtual void Update(TEntity entity)
        {
            this.DbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            this.DbSet.UpdateRange(entities);
        }

        public virtual void Delete(TEntity entity)
        {
            this.DbSet.Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            this.DbSet.RemoveRange(entities);
        }

        public void Dispose()
        {
        }
    }
}
