using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Enums;
using IdentityService.Models;

namespace IdentityService.Contracts
{
    public interface IUserRepository
    {
        Task<Users> Register(Users users);
        Task<Users> GetUserByEmail(string email);
        Task<HashSet<PermissionsEnum>> GetUserPermissions(Guid userId);
    }
}