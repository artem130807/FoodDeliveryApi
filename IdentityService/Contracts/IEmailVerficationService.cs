using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace IdentityService.Contracts
{
    public interface IEmailVerficationService
    {
        Task<bool> Verificate(string Code, string Email);
        Task SendVerificationAsync(string Email);
        Task DeleteEmailVerificate(string Email);
    }
}