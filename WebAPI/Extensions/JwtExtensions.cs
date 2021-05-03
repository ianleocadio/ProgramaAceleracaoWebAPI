using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System;

namespace WebAPI.Extensions
{
    public static class JwtExtensions
    {
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
    }
}
