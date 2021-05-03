using Domain.Exceptions;
using Domain.Models;
using Domain.Transfers;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUserService;

        public AuthenticationService(
            IUserRepository userRepository,
            ITokenService tokenService,
            ICurrentUserService currentUserService
        )
        {
            this._userRepository = userRepository;
            this._tokenService = tokenService;
            this._currentUserService = currentUserService;
        }

        public async Task<string> AuthenticateAsync(AuthTransfer auth, CancellationToken cancellationToken)
        {
            var user = await this._userRepository.GetByUsernameAsync(auth.Username, cancellationToken);

            if (user == null)
            {
                throw new AuthenticationException("Usuário incorreto.");
            }

            if (!this.AuthenticatePassword(auth.Password, user.Password, user.Hash))
            {
                throw new AuthenticationException("Senha incorreta.");
            }

            return await this._tokenService.GetTokenAsync(user, cancellationToken);
        }

        public async Task<string> RenewTokenAsync(CancellationToken cancellationToken)
        {
            var currentUser = await this.GetCurrentUserAsync(cancellationToken);

            return await this._tokenService.GetTokenAsync(currentUser, cancellationToken);
        }

        public async Task ChangePasswordAsync(AuthChangePasswordTransfer authChangePassword, CancellationToken cancellationToken)
        {
            var currentUser = await this.GetCurrentUserAsync(cancellationToken);

            if (authChangePassword.NewPassword != authChangePassword.RepeatNewPassword)
            {
                throw new AuthenticationException("Nova senha e sua repetição divergem.");
            }

            if (!this.AuthenticatePassword(authChangePassword.CurrentPassword, currentUser.Password, currentUser.Hash))
            {
                throw new AuthenticationException("Senha atual incorreta.");
            }

            var newHash = this.CreateHash();
            var encryptedNewPassword = this.EncryptPassword(authChangePassword.NewPassword, newHash);

            currentUser.Password = encryptedNewPassword;
            currentUser.Hash = newHash;

            await this._userRepository.UpdateAsync(currentUser, cancellationToken);
        }

        public string EncryptPassword(string password, string hash)
        {
            string passwordPlus = password + hash + password.Length;

            string passwordSHA256 = CryptographySHA256(passwordPlus);

            passwordSHA256 = CryptographySHA256(passwordSHA256 + hash + passwordSHA256.Length);

            return passwordSHA256;
        }

        public string CreateHash()
        {
            return Guid.NewGuid().ToString();
        }

        #region Auxiliary Methods

        private async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            var currentUser = await this._currentUserService.GetUserAsync(cancellationToken);

            if (currentUser == null)
            {
                throw new ActionRejectedException("Usuário não identificado.");
            }

            return currentUser!;
        }

        private bool AuthenticatePassword(string passwordEntered, string currentPassword, string hash)
        {
            var encryptedPassword = this.EncryptPassword(passwordEntered, hash);

            return encryptedPassword == currentPassword;
        }

        private string CryptographySHA256(string valor)
        {
            StringBuilder builder = new();

            using (SHA256 objSHA = SHA256.Create())
            {
                Encoding objEncoding = Encoding.UTF8;
                byte[] hash = objSHA.ComputeHash(objEncoding.GetBytes(valor));

                foreach (byte b in hash)
                {
                    builder.Append(b.ToString("x2"));
                }
            }

            return builder.ToString();
        }
        #endregion


    }
}
