using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IJwtService
    {
        string CreateToken(User user, IEnumerable<Claim> claimsIdentity);

        (SecurityKey signingKey, string audience, string issuer) GetTokenBearerParameters();
    }
}
