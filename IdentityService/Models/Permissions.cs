using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Enums;

namespace IdentityService.Models
{
    public class Permissions
    {
        public int Id { get; set; }
        public PermissionsEnum[] permissionsEnum { get; set; } 
    }
}