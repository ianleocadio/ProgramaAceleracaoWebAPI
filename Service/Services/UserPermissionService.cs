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
        private readonly IUserRepository _userRepository;
        private readonly IUserPermissionRepository _userPermissionRepository;

        public UserPermissionService(IUserRepository userRepository, IUserPermissionRepository userPermissionRepository)
        {
            this._userRepository = userRepository;
            this._userPermissionRepository = userPermissionRepository;
        }

        public async Task AddAsync(IEnumerable<UserPermissionUpdateTransfer> permissionUpdateTransfers, CancellationToken cancellationToken)
        {
            List<UserPermission> permissionsToAdd = new();

            foreach (var permissionUpdate in permissionUpdateTransfers)
            {
                User? user = await this._userRepository.GetByIdAsync(permissionUpdate.UserID, cancellationToken);

                if (user == null)
                {
                    throw new ActionRejectedException($"Usuário {permissionUpdate.UserID} inexistente.");
                }

                var newPermissionsEnum = permissionUpdate.Permissions
                .Except(
                    user.Permissions ?? Enumerable.Empty<UserPermission>(),
                    p => p,
                    up => up.Permission
                )
                .Where(p => Enum.IsDefined(p));

                var newPermissions = newPermissionsEnum.Select(p => new UserPermission()
                {
                    UserID = user.ID!.Value,
                    Permission = p
                });

                permissionsToAdd.AddRange(newPermissions);
            }

            await this._userPermissionRepository.CreateRangeAsync(permissionsToAdd, cancellationToken);
        }

        public async Task RemoveAsync(IEnumerable<UserPermissionUpdateTransfer> permissionUpdateTransfers, CancellationToken cancellationToken)
        {
            List<UserPermission> permissionsToRemove = new();

            foreach (var permissionUpdate in permissionUpdateTransfers)
            {
                var userPermissions = await this._userPermissionRepository.GetByUserIdAsync(permissionUpdate.UserID, cancellationToken);

                if (userPermissions == null || !userPermissions.Any())
                {
                    continue;
                }

                var oldPermissions = userPermissions.Intersect(permissionUpdate.Permissions, up => up.Permission, p => p);

                permissionsToRemove.AddRange(oldPermissions);
            }

            await this._userPermissionRepository.DeleteRangeAsync(permissionsToRemove, cancellationToken);
        }
    }
}
