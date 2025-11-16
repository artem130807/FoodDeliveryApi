using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Authorization;
using IdentityService.Configurations;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options, IOptions<AuthorizationOptions> authOptions):DbContext(options)
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RolePermissionsEntity> RolePermissions { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
          
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfigurations());
            modelBuilder.ApplyConfiguration(new RolesConfigurations());
            modelBuilder.ApplyConfiguration(new PermissionsConfigurations());
            modelBuilder.ApplyConfiguration(new RolesPermissionsCofigurations(authOptions.Value));
            modelBuilder.ApplyConfiguration(new EmailVerificationConfigurations());
            base.OnModelCreating(modelBuilder);
        }
    }
}