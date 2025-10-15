using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Authorization;
using IdentityService.Enums;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Configurations
{
    public class RolesPermissionsCofigurations : IEntityTypeConfiguration<Models.RolePermissionsEntity>
    {
        private readonly AuthorizationOptions _authorizationOptions;
        public RolesPermissionsCofigurations(AuthorizationOptions authorizationOptions)
        {
            _authorizationOptions = authorizationOptions;
        }

        public void Configure(EntityTypeBuilder<Models.RolePermissionsEntity> builder)
        {
            builder.ToTable("RolePermissions");
            builder.HasKey(x => new { x.RoleId, x.PermissionId });
            builder.HasData(ParseRolePermissions());
        }

        public List<Models.RolePermissionsEntity> ParseRolePermissions()
        {
            return _authorizationOptions.RolePermissions.SelectMany(rp => rp.Permissions.Select(p => new Models.RolePermissionsEntity
            {
                RoleId = (int)Enum.Parse<RolesEnum>(rp.Role),
                PermissionId = (int)Enum.Parse<PermissionsEnum>(p)
            })).ToList();
        }
    }
}