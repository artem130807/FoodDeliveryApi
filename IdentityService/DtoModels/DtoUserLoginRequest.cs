using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DtoModels
{
    public class DtoUserLoginRequest
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}