using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Models;
using Domain.Transfers;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : IUserService,
        IEntityConverterService<User, UserCreateTransfer>,
        IEntityConverterService<User, UserUpdateTransfer>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authService;

        public UserService(
            Repository.Interfaces.IUserRepository userRepository, 
            IAuthenticationService authenticationService
        )
        {
            this._userRepository = userRepository;
            this._authService = authenticationService;
        }

        public async Task<User> CreateAsync(UserCreateTransfer newUser, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(newUser.Email) && !newUser.Email.IsValidEmail())
            {
                throw new ActionRejectedException("E-mail inválido.");
            }

            if (await this._userRepository.ExistsByUsernameAsync(newUser.Username, cancellationToken))
            {
                throw new ActionRejectedException("Usuário já existente.");
            }

            User user = this.TransferToEntity(newUser);

            await this._userRepository.CreateAsync(user, cancellationToken);

            return user;
        }

        public async Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await this._userRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this._userRepository.GetAllAsync(cancellationToken);
        }

        public async Task<User> UpdateAsync(UserUpdateTransfer updateUser, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(updateUser.Email) && !updateUser.Email.IsValidEmail())
            {
                throw new ActionRejectedException("E-mail inválido.");
            }

            User? currentUser = await this._userRepository.GetByIdAsync(updateUser.ID, cancellationToken);

            if (currentUser == null)
            {
                throw new ActionRejectedException("Usuário não existente.");
            }

            currentUser = this.TransferToEntity(updateUser, currentUser);

            await this._userRepository.UpdateAsync(currentUser, cancellationToken);

            return currentUser;
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var user = await this.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                throw new ActionRejectedException("Usuário não existente.");
            }

            await this._userRepository.DeleteAsync(user, cancellationToken);
        }

        #region Auxiliary Methods

        public User TransferToEntity(UserCreateTransfer transfer, User entity)
        {
            var hash = this._authService.CreateHash();

            var encryptedPassword = this._authService.EncryptPassword(transfer.Password, hash);

            return new User()
            {
                Username = transfer.Username,
                Password = encryptedPassword,
                Hash = hash,
                Name = transfer.Name,
                Lastname = transfer.Lastname,
                Email = transfer.Email,
                Permissions = transfer.Permissions?.Select(permission => new UserPermission
                {
                    Permission = permission
                }).ToList()
            };
        }

        public User TransferToEntity(UserCreateTransfer transfer)
        {
            return this.TransferToEntity(transfer, new User());
        }

        public User TransferToEntity(UserUpdateTransfer transfer, User entity)
        {
            entity.Name = transfer.Name;
            entity.Lastname = transfer.Lastname;
            entity.Email = transfer.Email;

            return entity;
        }

        public User TransferToEntity(UserUpdateTransfer transfer)
        {
            return this.TransferToEntity(transfer, new User());
        }
        #endregion
    }
}
