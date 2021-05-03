using Domain.Models;
using Microsoft.AspNetCore.Http;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _uow;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork uow
        )
        {
            this._httpContextAccessor = httpContextAccessor;
            this._uow = uow;
        }

        public long? GetUserID()
        {
            return this.GetClaimValue<long?>(ClaimTypes.Sid);
        }

        public string? GetUsername()
        {
            return this.GetClaimValue<string?>(ClaimTypes.NameIdentifier);
        }

        public async Task<User?> GetUserAsync(CancellationToken cancellationToken)
        {
            if (!this.GetUserID().HasValue)
            {
                return null;
            }

            return await this._uow.UserRepository.GetByIdAsync(this.GetUserID()!.Value, cancellationToken);
        }

        #region Auxiliary Methods
        private T GetClaimValue<T>(string claimType)
        {
            Claim? claim = this._httpContextAccessor.HttpContext.User.FindFirst(claimType);

            if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
            {
                return default!;
            }

            if (claim.Value is T value)
            {
                return value;
            }

            try
            {
                if (Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(claim.Value);
                }

                return (T)Convert.ChangeType(claim.Value, typeof(T));
            }
            catch
            {
                return default!;
            }
        }
        #endregion
    }
}
