using Domain.Models;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IJwtService _jwtService;
        private readonly IAuthorizationService _authorizationService;

        public TokenService(
            IJwtService jwtService, 
            IAuthorizationService authorizationService
        )
        {
            this._jwtService = jwtService;
            this._authorizationService = authorizationService;
        }

        public async Task<string> GetTokenAsync(User user, CancellationToken cancellationToken)
        {
            IEnumerable<Claim> claimsPermissoes = await this._authorizationService.GetUserPermissionClaimsAsync(user.ID!.Value, cancellationToken);
            return GetUserTokenByPermissoes(user, claimsPermissoes.ToList());
        }

        private string GetUserTokenByPermissoes(User user, ICollection<Claim> claimsIdentity)
        {
            claimsIdentity.Add(new Claim(ClaimTypes.Sid, user.ID!.Value.ToString()));
            claimsIdentity.Add(new Claim(ClaimTypes.NameIdentifier, user.Username ?? string.Empty));

            return this._jwtService.CreateToken(user, claimsIdentity);
        }
    }
}
