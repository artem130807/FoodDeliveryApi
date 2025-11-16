using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DtoModels
{
    public class SendVerificationRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}