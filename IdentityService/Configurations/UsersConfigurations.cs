using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Configurations
{
    public class UsersConfigurations : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name);
            builder.Property(x => x.Email);
            builder.Property(X => X.PasswordHash);
            
            builder.HasMany(x => x.Roles).WithMany(x => x.Users).UsingEntity<UserRoles>
            (x => x.HasOne<Roles>().WithMany().HasForeignKey(x => x.RoleId),
            x => x.HasOne<Users>().WithMany().HasForeignKey(x => x.UserId)
            );
        }
    }
}