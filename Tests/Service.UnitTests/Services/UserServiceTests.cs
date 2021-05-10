using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.Transfers;
using FluentAssertions;
using NSubstitute;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Service.UnitTests.Services
{
    public class UserServiceTests
    {
        public class CreateAsync
        {
            [Fact]
            public async Task CreateValidUser_Should_ReturnNewUser()
            {
                // Arrage
                var cancellationToken = new CancellationToken();
                var authService = Substitute.For<IAuthenticationService>();
                var uow = Substitute.For<IUnitOfWork>();
                var userRepository = Substitute.For<IUserRepository>();

                var userTransfer = new UserCreateTransfer() 
                {
                    Username = "user",
                    Email = "user@email.com",
                    Name = "name user",
                    Lastname = "lastname user",
                    Password = "123",
                    Permissions = new List<PermissionEnum>()
                    {
                        PermissionEnum.UserCreate,
                        PermissionEnum.UserRead,
                        PermissionEnum.UserUpdate,
                        PermissionEnum.UserDelete,
                    }
                };

                uow.UserRepository.Returns(userRepository);

                userRepository.ExistsByUsernameAsync(userTransfer.Username, cancellationToken)
                    .Returns(false);

                var userService = new UserService(authService, uow);

                // Act
                var newUser = await userService.CreateAsync(userTransfer, cancellationToken);

                // Assert
                newUser.Should().NotBeNull();
                newUser.Username.Should().Equals(userTransfer.Username);
                newUser.Email.Should().Equals(userTransfer.Email);
                newUser.Name.Should().Equals(userTransfer.Name);
                newUser.Lastname.Should().Equals(userTransfer.Lastname);
                newUser.Permissions.Select(p => p.Permission).Should().Equal(userTransfer.Permissions);
            }

            [Fact]
            public async Task UserWithInvalidEmail_Should_ThrowActionRejectedException()
            {
                // Arrage
                var cancellationToken = new CancellationToken();
                var authService = Substitute.For<IAuthenticationService>();
                var uow = Substitute.For<IUnitOfWork>();

                var invalidEmail = " ";

                var userTransfer = new UserCreateTransfer()
                {
                    Username = "user",
                    Email = invalidEmail,
                    Name = "name user",
                    Lastname = "lastname user",
                    Password = "123",
                    Permissions = new List<PermissionEnum>()
                    {
                        PermissionEnum.UserCreate,
                        PermissionEnum.UserRead,
                        PermissionEnum.UserUpdate,
                        PermissionEnum.UserDelete,
                    }
                };

                var userService = new UserService(authService, uow);

                // Act
                Func<Task> act = async () => await userService.CreateAsync(userTransfer, cancellationToken);

                // Assert
                await act.Should().ThrowAsync<ActionRejectedException>();
            }

            [Fact]
            public async Task UsernameAlreadyExists_Should_ThrowActionRejectedException()
            {
                // Arrage
                var cancellationToken = new CancellationToken();
                var authService = Substitute.For<IAuthenticationService>();
                var uow = Substitute.For<IUnitOfWork>();
                var userRepository = Substitute.For<IUserRepository>();

                var userTransfer = new UserCreateTransfer()
                {
                    Username = "user",
                    Email = "user@email.com",
                    Name = "name user",
                    Lastname = "lastname user",
                    Password = "123",
                    Permissions = new List<PermissionEnum>()
                    {
                        PermissionEnum.UserCreate,
                        PermissionEnum.UserRead,
                        PermissionEnum.UserUpdate,
                        PermissionEnum.UserDelete,
                    }
                };

                uow.UserRepository.Returns(userRepository);

                userRepository.ExistsByUsernameAsync(userTransfer.Username, cancellationToken)
                    .Returns(true);

                var userService = new UserService(authService, uow);

                // Act
                Func<Task> act = async () => await userService.CreateAsync(userTransfer, cancellationToken);

                // Assert
                await act.Should().ThrowAsync<ActionRejectedException>();
            }
        }
    
        public class UpdateAsync
        {
            [Fact]
            public async Task UpdateValidUser_Should_ReturnUpdatedUser()
            {
                // Arrange
                var cancellationToken = new CancellationToken();
                var authService = Substitute.For<IAuthenticationService>();
                var uow = Substitute.For<IUnitOfWork>();
                var userRepository = Substitute.For<IUserRepository>();

                var currentUser = new User
                {
                    ID = 1,
                    Username = "username",
                    Email = "user@email.com",
                    Name = "currentName",
                    Lastname = "currentLastname",
                    Permissions = new List<UserPermission>(),
                };

                var userUpdateTransfer = new UserUpdateTransfer
                {
                    ID = 1,
                    Name = "newName",
                    Lastname = "newLastname",
                    Email = "newUser@email.com"
                };

                uow.UserRepository.Returns(userRepository);

                userRepository.GetByIdAsync(currentUser.ID!.Value, cancellationToken)
                    .Returns(currentUser);

                var userService = new UserService(authService, uow);

                // Act
                var updatedUser = await userService.UpdateAsync(userUpdateTransfer, cancellationToken);

                // Assert
                updatedUser.Should().NotBeNull();
                updatedUser.Name.Should().Equals(userUpdateTransfer.Name);
                updatedUser.Lastname.Should().Equals(userUpdateTransfer.Lastname);
                updatedUser.Email.Should().Equals(userUpdateTransfer.Email);
            }

            [Fact]
            public async Task UpdateNotExistingUser_Should_ReturnThrowActionRejectedException()
            {
                // Arrange
                var cancellationToken = new CancellationToken();
                var authService = Substitute.For<IAuthenticationService>();
                var uow = Substitute.For<IUnitOfWork>();

                var userUpdateTransfer = new UserUpdateTransfer
                {
                    ID = 1,
                    Name = "newName",
                    Lastname = "newLastname",
                    Email = "user@email.com"
                };

                var userService = new UserService(authService, uow);

                // Act
                Func<Task> act = async () => await userService.UpdateAsync(userUpdateTransfer, cancellationToken);

                // Assert
                await act.Should().ThrowAsync<ActionRejectedException>();
            }

            [Fact]
            public async Task UserWithInvalidEmail_Should_ReturnThrowActionRejectedException()
            {
                // Arrange
                var cancellationToken = new CancellationToken();
                var authService = Substitute.For<IAuthenticationService>();
                var uow = Substitute.For<IUnitOfWork>();

                var invalidEmail = "emailinvalido";

                var userUpdateTransfer = new UserUpdateTransfer
                {
                    ID = 1,
                    Name = "newName",
                    Lastname = "newLastname",
                    Email = invalidEmail
                };

                var userService = new UserService(authService, uow);

                // Act
                Func<Task> act = async () => await userService.UpdateAsync(userUpdateTransfer, cancellationToken);

                // Assert
                await act.Should().ThrowAsync<ActionRejectedException>();
            }

        }
        
        public class DeleteAsync
        {
            [Fact]
            public async Task DeleteValidUser_Should_NotThrowActionRejectedException()
            {
                // Arrange
                var cancellationToken = new CancellationToken();
                var authService = Substitute.For<IAuthenticationService>();
                var uow = Substitute.For<IUnitOfWork>();
                var userRepository = Substitute.For<IUserRepository>();

                var userID = 1;

                var currentUser = new User
                {
                    ID = userID,
                    Username = "username",
                    Email = "user@email.com",
                    Name = "currentName",
                    Lastname = "currentLastname",
                    Permissions = new List<UserPermission>(),
                };

                uow.UserRepository.Returns(userRepository);

                userRepository.GetByIdAsync(currentUser.ID!.Value, cancellationToken)
                    .Returns(currentUser);

                var userService = new UserService(authService, uow);

                // Act
                Func<Task> act = async () => await userService.DeleteAsync(userID, cancellationToken);

                // Assert
                await act.Should().NotThrowAsync<ActionRejectedException>();
            }

            [Fact]
            public async Task DeleteNotExistingUser_Should_ThrowActionRejectedException()
            {
                // Arrange
                var cancellationToken = new CancellationToken();
                var authService = Substitute.For<IAuthenticationService>();
                var uow = Substitute.For<IUnitOfWork>();

                var userID = 1;

                var userService = new UserService(authService, uow);

                // Act
                Func<Task> act = async () => await userService.DeleteAsync(userID, cancellationToken);

                // Assert
                await act.Should().ThrowAsync<ActionRejectedException>();
            }
        }
    
    }
}
