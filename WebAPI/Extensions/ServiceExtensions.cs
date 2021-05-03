using Domain.Transfers.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Service.Interfaces;
using Service.Services;
using WebAPI.AutoMapper;

namespace WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<ICurrentUserService, CurrentUserService>()
                .AddScoped<IJwtService, JwtService>()
                .AddScoped<ITokenService, TokenService>()
                .AddScoped<IAuthorizationService, AuthorizationService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                // Application Services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IUserPermissionService, UserPermissionService>();
        }

        public static IServiceCollection ConfigureWritables(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .ConfigureWritable<Secrets>(configuration.GetSection("secrets"), "appsecrets.json");
        }

        public static IServiceCollection ConfigureWritable<T>(this IServiceCollection services, IConfigurationSection section, string file) where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableConfiguration<T>>(provider =>
            {
                var configuration = (IConfigurationRoot)provider.GetRequiredService<IConfiguration>();
                var environment = provider.GetRequiredService<IHostEnvironment>();
                var options = provider.GetRequiredService<IOptionsMonitor<T>>();
                return new WritableConfiguration<T>(environment, options, configuration, section.Key, file);
            });

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper((provider, cfg) => AutoMapperConfig.Configure(provider, cfg), typeof(Startup));
        }
    }
}
