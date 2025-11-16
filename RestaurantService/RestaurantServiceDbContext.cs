using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Configurations;
using RestaurantService.Models;

namespace RestaurantService
{
    public class RestaurantServiceDbContext(DbContextOptions<RestaurantServiceDbContext> options): DbContext(options)
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RestaurantConfigurations());
            modelBuilder.ApplyConfiguration(new MenuCategoryConfigurations());
            modelBuilder.ApplyConfiguration(new MenuItemConfigurations());        
            base.OnModelCreating(modelBuilder);
        }
    }
}