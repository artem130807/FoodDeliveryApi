using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityService.Authorization;
using IdentityService.Enums;
using IdentityService.Provider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService
{
    public static class Extensions
    {
        public static void AddDb(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<IdentityDbContext>(o =>
            {
                o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
        public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtoptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.SecretKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["tasty"];
                        return Task.CompletedTask;
                    }
                };

            });
             services.AddScoped<IPermissionService, PermissionsService>();
             services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
             services.AddAuthorization();
        }
        public static IEndpointConventionBuilder RequirePermissions<TBuilder>
        (this TBuilder builder, params PermissionsEnum[] permissions) where TBuilder : IEndpointConventionBuilder
        {
            return builder.RequireAuthorization(policy => policy.AddRequirements(new PermissionRequirement(permissions)));
        }
    }
}