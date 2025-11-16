using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantService.Models;

namespace RestaurantService.Configurations
{
    public class MenuItemConfigurations : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItems");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);
            builder.Property(x => x.Description)
            .HasMaxLength(1000);
            builder.Property(x => x.Price)
            .HasColumnType("decimal(10,2)") 
            .IsRequired();
            builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);
            builder.Property(x => x.IsAvailable)
            .HasDefaultValue(true);

            builder.HasOne(x => x.MenuCategory)
            .WithMany(x => x.MenuItems)
            .HasForeignKey(x => x.CategoryId);
            builder.HasOne(x => x.Restaurant)
            .WithMany()
            .HasForeignKey(x => x.RestaurantId);
            
        }
    }
}