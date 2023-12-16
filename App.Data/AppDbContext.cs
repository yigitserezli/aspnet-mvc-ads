using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AdvertCommentsEntity> AdvertComments { get; set; }
        public DbSet<AdvertEntity> Adverts { get; set; }
        public DbSet<BlogCommentsEntity> BlogComments { get; set; }
        public DbSet<BlogEntity> Blogs { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CustomerFavListentity> CustomerFavLists { get; set; }
        public DbSet<OrderstEntity> Orders { get; set; }
        public DbSet<RoleEntity> RoleEntities { get; set; }
        public DbSet<UserEntity> Users { get; set; } 
        public DbSet<UserRoleEntity> UserRoles { get; set; }


      
       

        public DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdvertCommentsEntity>().HasOne(c => c.User).WithMany(z => z.AdvertComments).HasForeignKey(c=>c.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AdvertCommentsEntity>().HasOne(c => c.Advert).WithMany(z => z.AdvertComments).HasForeignKey(c => c.AdvertId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CustomerFavListentity>().HasOne(c => c.User).WithMany(z => z.CustomerFavs).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderstEntity>().HasOne(c => c.User).WithMany(z => z.Orders).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderstEntity>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderstEntity>().HasOne(o => o.Advert).WithMany(a => a.Orders).HasForeignKey(o => o.AdvertId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BlogCommentsEntity>().HasOne(b => b.User).WithMany(u => u.BlogComments).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade);

        }



    }
}
