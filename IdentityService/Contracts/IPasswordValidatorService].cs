using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Records;

namespace IdentityService.Contracts
{
    public interface IPasswordValidatorService
    {
        Task<PasswordValidationResult> ValidatePasswordAsync(string password);
    }
}