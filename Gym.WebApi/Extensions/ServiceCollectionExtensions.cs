using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Gym.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT_SECRET"]!)),
                        ValidateIssuer = true,
                        ValidIssuer = "Gym.WebApi",
                        ValidateAudience = true,
                        ValidAudience = "Gym.WebApplication",
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RoleClaimType = ClaimTypes.Role
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = async (context) =>
                        {
                            if (context.Request.Cookies.TryGetValue("accessToken", out var cookieToken))
                            {
                                context.Token = cookieToken;
                            }
                        },

                        OnAuthenticationFailed = async (context) => { },

                        OnTokenValidated = async (context) => { }
                    };

                    options.SaveToken = true;
                    options.IncludeErrorDetails = true;
                });

            return services;
        }

        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(nameof(SecurityPolicy.RequireAuthenticated), policy => policy.RequireAuthenticatedUser())
                .AddPolicy(nameof(SecurityPolicy.AdminOnly), policy => policy.RequireRole("Admin"))
                .AddPolicy(nameof(SecurityPolicy.ClientOnly), policy => policy.RequireRole("Client"));

            return services;
        }

        public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(nameof(CorsPolicy.AllowWebApplication),
                    policy =>
                    {
                        policy.WithOrigins(configuration["WebApplicationUrl"]!)
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            return services;
        }
    }
}
