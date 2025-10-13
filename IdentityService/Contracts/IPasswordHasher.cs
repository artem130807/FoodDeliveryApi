using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Contracts
{
    public interface IPasswordHasher
    {
        string Generate(string Password);
        bool Verify(string Password, string PasswordHash);
    }
}