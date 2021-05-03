using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        #region Repositories
        public IUserRepository UserRepository { get; }
        public IUserPermissionRepository UserPermissionRepository { get; }
        #endregion

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
