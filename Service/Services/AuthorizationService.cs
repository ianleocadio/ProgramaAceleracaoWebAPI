using Domain.Models;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUnitOfWork _uow;

        public AuthorizationService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<IEnumerable<Claim>> GetUserPermissionClaimsAsync(long userID, CancellationToken cancellationToken)
        {
            var userPermissions = await this._uow.UserPermissionRepository.GetByUserIdAsync(userID, cancellationToken);

            if (userPermissions == null || !userPermissions.Any())
            {
                return Enumerable.Empty<Claim>();
            }

            var claims = new List<Claim>();

            foreach (UserPermission userPermission in userPermissions)
            {
                var permissionKey = (int)userPermission.Permission;

                if (claims.Any(c => c.Type == ClaimTypes.Role.ToString() && c.Value == permissionKey.ToString()))
                {
                    continue;
                }

                claims.Add(new Claim(ClaimTypes.Role.ToString(), permissionKey.ToString()));
            }

            return claims;
        }
    }
}
