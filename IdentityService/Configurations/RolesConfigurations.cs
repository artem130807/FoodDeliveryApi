using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Enums;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Configurations
{
    public class RolesConfigurations : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name);
            builder.HasMany(x => x.Permissions).WithMany(x => x.Roles)
            .UsingEntity<RolePermissionsEntity>
            (x => x.HasOne<Permissions>().WithMany().HasForeignKey(x => x.PermissionId),
            x => x.HasOne<Roles>().WithMany().HasForeignKey(x => x.RoleId)
            );
            var roles = Enum.GetValues<RolesEnum>().
            Select(x => new Roles { Id = (int)x, Name = x.ToString()});
            builder.HasData(roles);
        }
    }
}