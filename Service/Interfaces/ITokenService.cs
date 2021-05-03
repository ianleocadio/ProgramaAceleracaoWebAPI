using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(User user, CancellationToken cancellationToken);
    }
}
