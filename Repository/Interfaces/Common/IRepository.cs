using Domain.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Interfaces.Common
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class, IEntity
    {
        Task CreateAsync(TEntity entity, CancellationToken cancellationToken);

        Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken);

        Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
