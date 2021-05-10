using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Models;
using Domain.Transfers;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Interfaces.Common;
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
        private readonly IAuthenticationService _authService;
        private readonly IUnitOfWork _uow;

        public UserService(
            IAuthenticationService authenticationService,
            IUnitOfWork uow
        )
        {
            this._authService = authenticationService;
            this._uow = uow;
        }

        public async Task<User> CreateAsync(UserCreateTransfer newUser, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(newUser.Email) && !newUser.Email.IsValidEmail())
            {
                throw new ActionRejectedException("E-mail inválido.");
            }

            if (await this._uow.UserRepository.ExistsByUsernameAsync(newUser.Username, cancellationToken))
            {
                throw new ActionRejectedException("Usuário já existente.");
            }

            User user = this.TransferToEntity(newUser);

            await this._uow.UserRepository.CreateAsync(user, cancellationToken);

            await this._uow.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await this._uow.UserRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this._uow.UserRepository.GetAllAsync(cancellationToken);
        }

        public async Task<User> UpdateAsync(UserUpdateTransfer updateUser, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(updateUser.Email) && !updateUser.Email.IsValidEmail())
            {
                throw new ActionRejectedException("E-mail inválido.");
            }

            User? currentUser = await this._uow.UserRepository.GetByIdAsync(updateUser.ID, cancellationToken);

            if (currentUser == null)
            {
                throw new ActionRejectedException("Usuário não existente.");
            }

            currentUser = this.TransferToEntity(updateUser, currentUser);

            this._uow.UserRepository.Update(currentUser);

            await this._uow.SaveChangesAsync(cancellationToken);

            return currentUser;
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var user = await this.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                throw new ActionRejectedException("Usuário não existente.");
            }

            this._uow.UserRepository.Delete(user);

            await this._uow.SaveChangesAsync(cancellationToken);
        }

        #region Auxiliary Methods

        private (string hash, string encryptedPassword) EncryptPassword(string password)
        {
            var hash = this._authService.CreateHash();

            var encryptedPassword = this._authService.EncryptPassword(password, hash);

            return (hash, encryptedPassword);
        }

        public User TransferToEntity(UserCreateTransfer transfer, User entity)
        {
            var (hash, encryptedPassword) = this.EncryptPassword(transfer.Password);

            return new User()
            {
                Username = transfer.Username,
                Password = encryptedPassword,
                Hash = hash,
                Name = transfer.Name,
                Lastname = transfer.Lastname,
                Email = transfer.Email,
                Permissions = transfer.Permissions.Select(permission => new UserPermission
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
