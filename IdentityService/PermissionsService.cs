using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Authorization;
using IdentityService.Contracts;
using IdentityService.Enums;
using IdentityService.Repositories;

namespace IdentityService
{
    public class PermissionsService : IPermissionService
    {
        private readonly IUserRepository _userRepository;
        public PermissionsService(IUserRepository usersRepository)
        {
            _userRepository = usersRepository;
        }
        public Task<HashSet<PermissionsEnum>> GetPermissionsAsync(Guid userId)
        {
            return _userRepository.GetUserPermissions(userId);
        }
    }
}