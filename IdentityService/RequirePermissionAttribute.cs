using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Enums;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService
{
    public class RequirePermissionAttribute:AuthorizeAttribute
    {
        public RequirePermissionAttribute(params PermissionsEnum[] permissionsEnum)
        {
            Policy = $"Permission:{permissionsEnum}";
        }
    }
}