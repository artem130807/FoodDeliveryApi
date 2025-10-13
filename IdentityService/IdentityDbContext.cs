using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Configurations;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityService
{
    public class IdentityDbContext:DbContext
    {
        public DbSet<Users> Users { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> dbContextOptions) : base(dbContextOptions) { }

        public IdentityDbContext() { }
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfigurations());
            base.OnModelCreating(modelBuilder);
        }
    }
}