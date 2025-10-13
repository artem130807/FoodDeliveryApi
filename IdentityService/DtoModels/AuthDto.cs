using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DtoModels
{
    public class AuthDto
    {
        public string Token { get; set; }
        public string Message { get; set; }
    }
}