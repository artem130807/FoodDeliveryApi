using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Records;
using IdentityService.Service;

namespace IdentityService.Contracts
{
    public interface IDnsEmailValidator
    {
        Task<EmailValidationResult> ValidateEmailAsync(string Email);
    }
}