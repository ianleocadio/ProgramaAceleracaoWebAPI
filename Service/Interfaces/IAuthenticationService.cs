using Domain.Models;
using Domain.Transfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAsync(AuthTransfer auth, CancellationToken cancellationToken);

        Task<string> RenewTokenAsync(CancellationToken cancellationToken);

        Task ChangePasswordAsync(AuthChangePasswordTransfer authChangePassword, CancellationToken cancellationToken);

        string EncryptPassword(string password, string hash);

        string CreateHash();
    }
}
