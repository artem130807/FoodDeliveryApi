using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantService.Models;

namespace RestaurantService.Configurations
{
    public class RestaurantConfigurations : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("Restaurants");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);;
            builder.Property(x => x.Description)
            .HasMaxLength(1000);;
            builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(500);
            builder.Property(x => x.Street)
            .IsRequired()
            .HasMaxLength(255); ;
            builder.Property(x => x.TimeClose);
            builder.Property(x => x.TimeOpen);
            builder.Property(x => x.HouseNumber)
            .IsRequired()
            .HasMaxLength(10);
            builder.Property(x => x.Apartment)
            .HasMaxLength(20);
            builder.Property(x => x.Phone)
            .HasMaxLength(20); 
            builder.Property(x => x.CuisineType)
            .IsRequired()
            .HasMaxLength(100);
            builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);;
            builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
            builder.Property(x => x.Rating)
            .HasColumnType("decimal(3,2)")
            .HasDefaultValue(0.0m);
            builder.Property(x => x.RatingCount)
            .HasDefaultValue(0);
            builder.Property(x => x.DeliveryTime)
            .HasDefaultValue(30);
            builder.Property(x => x.DeliveryFee)
            .HasColumnType("decimal(10,2)")
            .HasDefaultValue(0.0m);
            builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");;
            builder.HasMany(x => x.MenuCategories)
            .WithOne(x => x.Restaurant)
            .HasForeignKey(x => x.RestaurantId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}