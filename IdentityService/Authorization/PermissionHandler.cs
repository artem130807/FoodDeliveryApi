using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Provider;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _scope;
        public PermissionHandler(IServiceScopeFactory scope)
        {
            _scope = scope;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == CustomClaims.UserId);
            if (userId is null || !Guid.TryParse(userId.Value, out var id))
            {
                return;
            }
            using var scope = _scope.CreateScope();
            var permissionsService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
            var permissions = await permissionsService.GetPermissionsAsync(id);
            if (permissions.Intersect(requirement.Permissions).Any())
            {
                context.Succeed(requirement);
            }
        }
    }
}