using Domain.Models;
using Repository.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserPermissionRepository : IRepository<UserPermission>
    {
        Task<IEnumerable<UserPermission>> GetByUserIdAsync(long userId, CancellationToken cancellationToken);
    }
}
