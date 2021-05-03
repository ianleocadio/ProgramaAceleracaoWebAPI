using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAuthorizationService
    {
        Task<IEnumerable<Claim>> GetUserPermissionClaimsAsync(long userID, CancellationToken cancellationToken);
    }
}
