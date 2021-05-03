using Domain.Exceptions;
using Domain.Models;
using Domain.Transfers;
using Domain.Extensions;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly IUnitOfWork _uow;

        public UserPermissionService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task AddAsync(IEnumerable<UserPermissionUpdateTransfer> permissionUpdateTransfers, CancellationToken cancellationToken)
        {
            List<UserPermission> permissionsToAdd = new();

            foreach (var permissionUpdate in permissionUpdateTransfers)
            {
                User? user = await this._uow.UserRepository.GetByIdAsync(permissionUpdate.UserID, cancellationToken);

                if (user == null)
                {
                    throw new ActionRejectedException($"Usuário {permissionUpdate.UserID} inexistente.");
                }

                var newPermissions = permissionUpdate.Permissions
                    .Except(
                        user.Permissions, 
                        p => p, 
                        up => up.Permission
                    )
                    .Where(p => Enum.IsDefined(p))
                    .Select(p => new UserPermission()
                    {
                        UserID = user.ID!.Value,
                        Permission = p
                    });

                permissionsToAdd.AddRange(newPermissions);
            }

            await this._uow.UserPermissionRepository.CreateRangeAsync(permissionsToAdd, cancellationToken);

            await this._uow.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(IEnumerable<UserPermissionUpdateTransfer> permissionUpdateTransfers, CancellationToken cancellationToken)
        {
            List<UserPermission> permissionsToRemove = new();

            foreach (var permissionUpdate in permissionUpdateTransfers)
            {
                var userPermissions = await this._uow.UserPermissionRepository.GetByUserIdAsync(permissionUpdate.UserID, cancellationToken);

                if (userPermissions == null || !userPermissions.Any())
                {
                    continue;
                }

                var oldPermissions = userPermissions
                    .Intersect(
                        permissionUpdate.Permissions, 
                        up => up.Permission, 
                        p => p
                    );

                permissionsToRemove.AddRange(oldPermissions);
            }

            this._uow.UserPermissionRepository.DeleteRange(permissionsToRemove);

            await this._uow.SaveChangesAsync(cancellationToken);
        }
    }
}
