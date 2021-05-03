using Database;
using Database.Contexts;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;

namespace WebAPI.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUserPermissionRepository, UserPermissionRepository>();
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<DbContextBase>(opts =>
                {
                    var sqlServerConnectionString = configuration.GetConnectionString("SQLServer");

                    if (string.IsNullOrWhiteSpace(sqlServerConnectionString))
                    {
                        opts.UseInMemoryDatabase("DATABASE_IN_MEMORY");
                        return;
                    }

                    opts.UseSqlServer(sqlServerConnectionString);
                });
        }
    }
}
