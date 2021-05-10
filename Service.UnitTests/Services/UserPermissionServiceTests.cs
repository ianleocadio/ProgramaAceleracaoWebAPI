using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.Transfers;
using FluentAssertions;
using NSubstitute;
using Repository.Interfaces;
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
    public class UserPermissionServiceTests
    {
        public class AddAsync
        {
            [Fact]
            public async Task AddValidPermission_Should_AddPermissionToUserCorrectly()
            {
                // Arrange
                var cancellationToken = new CancellationToken();
                var userRepository = Substitute.For<IUserRepository>();
                var uow = Substitute.For<IUnitOfWork>();

                var user_1 = new User
                {
                    ID = 1,
                    Permissions = new List<UserPermission>
                    {
                        new() { ID = 1, UserID = 1, Permission = PermissionEnum.UserRead },
                        new() { ID = 2, UserID = 1, Permission = PermissionEnum.UserUpdate }
                    }
                };

                var user_2 = new User
                {
                    ID = 2,
                    Permissions = new List<UserPermission>
                    {
                        new() { ID = 3, UserID = 2, Permission = PermissionEnum.UserRead },
                        new() { ID = 4, UserID = 3, Permission = PermissionEnum.UserUpdate }
                    }
                };

                var permissionUpdateTransfers = new List<UserPermissionUpdateTransfer>
                {
                    new()
                    {
                        UserID = user_1.ID!.Value,
                        Permissions = new List<PermissionEnum>()
                        {
                            PermissionEnum.UserCreate
                        }
                    },
                    new()
                    {
                        UserID = user_2.ID!.Value,
                        Permissions = new List<PermissionEnum>()
                        {
                            PermissionEnum.UserDelete
                        }
                    }
                };

                uow.UserRepository.Returns(userRepository);

                userRepository.GetByIdAsync(user_1.ID!.Value, cancellationToken)
                    .Returns(user_1);

                userRepository.GetByIdAsync(user_2.ID!.Value, cancellationToken)
                    .Returns(user_2);

                var userPermissionService = new UserPermissionService(uow);

                // Act
                Func<Task> act = async () => await userPermissionService.AddAsync(permissionUpdateTransfers, cancellationToken);

                // Assert
                await act.Should().NotThrowAsync<ActionRejectedException>();
            }
        }
    }
}
