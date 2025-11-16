using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantService.Models;

namespace RestaurantService.Configurations
{
    public class MenuCategoryConfigurations : IEntityTypeConfiguration<MenuCategory>
    {
        public void Configure(EntityTypeBuilder<MenuCategory> builder)
        {
            builder.ToTable("MenuCategories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);
            builder.Property(x => x.DisplayOrder);
            builder.Property(x => x.DisplayOrder)
            .HasDefaultValue(0);
            builder.HasOne(x => x.Restaurant)
            .WithMany(x => x.MenuCategories)
            .HasForeignKey(x => x.RestaurantId);         
        }
    }
}