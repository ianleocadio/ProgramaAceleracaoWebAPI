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
            await this.Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            await this.DbSet.AddRangeAsync(entities, cancellationToken);
            await this.Context.SaveChangesAsync(cancellationToken);
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

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            this.DbSet.Update(entity);
            await this.Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            this.DbSet.UpdateRange(entities);
            await this.Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            this.DbSet.Remove(entity);
            await this.Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            this.DbSet.RemoveRange(entities);
            await this.Context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
        }
    }
}
