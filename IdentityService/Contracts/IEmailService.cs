using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Contracts
{
    public interface IEmailService
    {
        Task SendVerificationService(string email, string code);
    }
}