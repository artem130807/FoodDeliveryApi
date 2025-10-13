using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Enums;
using Microsoft.Identity.Client;

namespace IdentityService.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public  RolesEnum rolesEnum { get; set; }
    }
}