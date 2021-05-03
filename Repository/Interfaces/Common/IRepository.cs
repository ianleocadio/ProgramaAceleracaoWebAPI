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

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    }
}
