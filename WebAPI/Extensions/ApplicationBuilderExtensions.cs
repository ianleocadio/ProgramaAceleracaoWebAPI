using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database.Contexts;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;

namespace WebAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public async static Task MigrateAsync(this IApplicationBuilder builder, CancellationToken cancellationToken)
        {
            using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DbContextBase>();

            if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await context.Database.EnsureCreatedAsync(cancellationToken);
                // Somente se trabalhar com Migrations
                // await context.Database.MigrateAsync(cancellationToken);
            }

            await CreateDefaultUsersAsync(scope.ServiceProvider, cancellationToken);
        }

        private async static Task CreateDefaultUsersAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            // O ideal seria utilizar o UserService com um método CreateRangeAsync().
            var userRepository = serviceProvider.GetRequiredService<IUserRepository>();

            if (await userRepository.ExistsByUsernameAsync("admin", cancellationToken))
            {
                return;
            }

            var defaultUsers = new List<User>()
            {
                new()
                {
                    Username = "admin",
                    // Password = admin
                    Password = "4e1273ebe0369e9e1cbfd696ef428e94cc964dfdd9e4aa1aa9adffd53d804e70",
                    Hash = "8e0c7bd7-d618-4d45-9a47-66ab45f47d8a",
                    Permissions = new List<UserPermission>()
                    {
                        new() { Permission = PermissionEnum.UserCreate },
                        new() { Permission = PermissionEnum.UserRead },
                        new() { Permission = PermissionEnum.UserUpdate },
                        new() { Permission = PermissionEnum.UserDelete },
                    }
                },
                new()
                {
                    Username = "consulta",
                    // Password = consulta
                    Password = "54bb71af397ddf8b1d1c98aa8913cc91406ddcde4751f9001663e61ebf8629fb",
                    Hash = "3815838c-e387-48d2-80a9-cd786392e4b0",
                    Permissions = new List<UserPermission>()
                    {
                        new() { Permission = PermissionEnum.UserRead },
                    }
                },
            };

            await userRepository.CreateRangeAsync(defaultUsers, cancellationToken);
        }
    }
}
