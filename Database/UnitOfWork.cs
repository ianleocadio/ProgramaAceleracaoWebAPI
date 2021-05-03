using Database.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContextBase _context;

        public UnitOfWork(IServiceProvider serviceProvider, DbContextBase dbContext)
        {
            this._serviceProvider = serviceProvider;
            this._context = dbContext;
        }

        #region Repositories
        public IUserRepository UserRepository => this._serviceProvider.GetRequiredService<IUserRepository>();
        public IUserPermissionRepository UserPermissionRepository => this._serviceProvider.GetRequiredService<IUserPermissionRepository>();
        #endregion

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await this._context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            this._context.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await this._context.DisposeAsync();
        }
    }
}
