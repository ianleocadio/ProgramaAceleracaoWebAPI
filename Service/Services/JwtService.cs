using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.Transfers.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Services
{
    public class JwtService : IJwtService
    {
        private static readonly string Issuer = "api";
        private static readonly string Audience = "client";

        private readonly IConfiguration _config;
        private readonly IWritableConfiguration<Secrets> _secretWriteableConfig;

        public JwtService(
            IConfiguration config,
            IWritableConfiguration<Secrets> secretWriteableConfig)
        {
            this._config = config;
            this._secretWriteableConfig = secretWriteableConfig;
        }

        public string CreateToken(User user, IEnumerable<Claim> claimsIdentity)
        {
            var claimsList = claimsIdentity.Where(c => c.Type != JwtRegisteredClaimNames.Aud).ToList();

            var payload = new JwtPayload(Issuer, Audience, claimsList, null, DateTime.Now.AddHours(4));

            var signingKey = this.GetSecurityKey();

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var header = new JwtHeader(signingCredentials);

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            var tokenString = handler.WriteToken(secToken);

            return tokenString;
        }

        public (SecurityKey signingKey, string audience, string issuer) GetTokenBearerParameters()
        {
            return (this.GetSecurityKey(), Audience, Issuer);
        }

        private SecurityKey GetSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.GetPrivateKey()));
        }

        private string GetPrivateKey()
        {
            var secrets = this._config.GetSection("Secrets").Get<Secrets>();

            if (secrets != null)
            {
                return secrets.Key;
            }

            using var cryptoProvider = new RSACryptoServiceProvider(2048);

            RSAParameters parametersKey = cryptoProvider.ExportParameters(true);
            
            string privateKey = parametersKey.P != null ? Convert.ToBase64String(parametersKey.P) : string.Empty;

            this._secretWriteableConfig.Update((a) =>
            {
                a.Key = privateKey;
                secrets = a;
            });

            return secrets?.Key ?? string.Empty;
        }
    }
}
