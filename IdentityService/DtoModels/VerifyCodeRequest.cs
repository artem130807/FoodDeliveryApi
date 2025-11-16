using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.DtoModels
{
    public class VerifyCodeRequest
    {
      public string Code { get; set; } = string.Empty;
    }
}