using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Models
{
    public class RolePermissionsEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}