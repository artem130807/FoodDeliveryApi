using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Enums;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Authorization
{
    public class PermissionRequirement(PermissionsEnum[] permissions):IAuthorizationRequirement
    {
        public PermissionsEnum[] Permissions = permissions;      
    }
}