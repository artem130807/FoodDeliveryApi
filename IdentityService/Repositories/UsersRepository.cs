using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Contracts;
using IdentityService.Enums;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;
        public UsersRepository(IdentityDbContext context)
        {
            _context = context;
        }
        public async Task<Users> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }

        public async Task<Users> Register(Users users)
        {
            _context.Users.Add(users);
            await _context.SaveChangesAsync();
            return users;
        }
        public async Task<HashSet<PermissionsEnum>> GetUserPermissions(Guid userId)
        {
            var roles = await _context.Users.AsNoTracking().Include(x => x.Roles).ThenInclude(x => x.Permissions).Where(x => x.Id == userId).Select(x => x.Roles).ToListAsync();
            return roles.SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => (PermissionsEnum)x.Id)
            .ToHashSet();
        }

        public async Task UpdatePasswordHash(string email, string password)
        {
            await _context.Users.Where(x => x.Email == email)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.PasswordHash, password));
            await _context.SaveChangesAsync();
        }

        public async Task<string> UpdateCity(Guid Id, string City)
        {
            await _context.Users.Where(x => x.Id == Id)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.City, City));
            await _context.SaveChangesAsync();
            return City;
        }
    }
}