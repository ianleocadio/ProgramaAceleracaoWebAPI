using Database.Contexts;
using Database.Repositories;
using Domain.Transfers.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Services;
using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.AutoMapper;

namespace WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddTransient<ICurrentUserService, CurrentUserService>();


            return services
                .AddScoped<IJwtService, JwtService>()
                .AddScoped<ITokenService, TokenService>()
                .AddScoped<Service.Interfaces.IAuthorizationService, AuthorizationService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IUserPermissionService, UserPermissionService>();
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

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IUserPermissionRepository, UserPermissionRepository>();
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

        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
        {
            IServiceProvider provider = services.BuildServiceProvider();
            IJwtService jwtService = provider.GetRequiredService<IJwtService>();

            (SecurityKey signingKey, string audience, string issuer) = jwtService.GetTokenBearerParameters();

            services
            .AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;

                paramsValidation.IssuerSigningKey = signingKey;
                paramsValidation.ValidAudience = audience;
                paramsValidation.ValidIssuer = issuer;
                paramsValidation.ValidateAudience = true;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;

            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(
                    "Bearer",
                    new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build()
                );
            });

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            return services.AddAutoMapper((provider, cfg) => AutoMapperConfig.Configure(provider, cfg), typeof(Startup));
        }
    }
}
