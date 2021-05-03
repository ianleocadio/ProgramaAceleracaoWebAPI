using Domain.Transfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserPermissionService
    {
        Task AddAsync(IEnumerable<UserPermissionUpdateTransfer> permissionUpdateTransfer, CancellationToken cancellationToken);
        Task RemoveAsync(IEnumerable<UserPermissionUpdateTransfer> permissionUpdateTransfer, CancellationToken cancellationToken);
    }
}
