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
        public DbSet<OrdersEntity> Orders { get; set; }
        public DbSet<RoleEntity> RoleEntities { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdvertCommentsEntity>().HasOne(c => c.User).WithMany(z => z.AdvertComments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<AdvertCommentsEntity>().HasOne(c => c.Advert).WithMany(z => z.AdvertComments).HasForeignKey(c => c.AdvertId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<CustomerFavListentity>().HasOne(c => c.User).WithMany(z => z.CustomerFavs).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<OrdersEntity>().HasOne(c => c.User).WithMany(z => z.Orders).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<OrdersEntity>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<OrdersEntity>().HasOne(o => o.Advert).WithMany(a => a.Orders).HasForeignKey(o => o.AdvertId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<BlogCommentsEntity>().HasOne(b => b.User).WithMany(u => u.BlogComments).HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<RoleEntity>().HasData(
                new RoleEntity
                {
                    Id = 1,
                    Name = "User",
                },
                new RoleEntity
                {
                    Id = 2,
                    Name = "Admin"
                }
                );

            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    Name = "Mahmut",
                    SurName = "Tuncer",
                    Address = "Maltepe No:10 Bursa",
                    Username = "test@test.com",
                    PasswordHash = "lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4="
                }


                );

            modelBuilder.Entity<UserRoleEntity>().HasData(
                new UserRoleEntity
                {
                    Id = 1,
                    RoleId = 1,
                    UserId = 1
                },
                new UserRoleEntity
                {
                    Id = 2,
                    RoleId = 2,
                    UserId = 1
                }


                );


            modelBuilder.Entity<CategoryEntity>().HasData(
                new CategoryEntity
                {
                    Id = 1,
                    Name = "Electronics",
                    parentCategoryID = null,
                    categoryPopularityIndex = 10

                },

                new CategoryEntity
                {
                    Id = 2,
                    Name = "Notebooks",
                    parentCategoryID = 1,
                    categoryPopularityIndex = 9

                },
                new CategoryEntity
                {
                    Id = 3,
                    Name = "Desktops",
                    parentCategoryID = 1,
                    categoryPopularityIndex = 8

                },
                new CategoryEntity
                {
                    Id = 4,
                    Name = "Smart Phones",
                    parentCategoryID = 1,
                    categoryPopularityIndex = 10

                },
                new CategoryEntity
                {
                    Id = 5,
                    Name = "Real Estate",
                    parentCategoryID = null,
                    categoryPopularityIndex = 2
                },
                new CategoryEntity
                {
                    Id = 6,
                    Name = "Apartments",
                    parentCategoryID = 5,
                    categoryPopularityIndex = 3
                },
                new CategoryEntity
                {
                    Id = 7,
                    Name = "Villas",
                    parentCategoryID = 5,
                    categoryPopularityIndex = 3
                },
                new CategoryEntity
                {
                    Id = 8,
                    Name = "Pets",
                    parentCategoryID = null,
                    categoryPopularityIndex = 2
                },
                new CategoryEntity
                {
                    Id = 9,
                    Name = "Cats",
                    parentCategoryID = 8,
                    categoryPopularityIndex = 2

                },
                new CategoryEntity
                {
                    Id = 10,
                    Name = "Dogs",
                    parentCategoryID = 8,
                    categoryPopularityIndex = 2

                },
                 new CategoryEntity
                 {
                     Id = 11,
                     Name = "Fish",
                     parentCategoryID = 8,
                     categoryPopularityIndex = 2

                 }
                );


            modelBuilder.Entity<AdvertEntity>().HasData(
              new AdvertEntity
              {
                  Id = 1,
                  Name = "MacBook Pro 13.3 inc M2 8CPU 10GPU 8GB 256GB SSD U",
                  Description = "Very nice, clean computer. It has 8 GB of RAM and a very fast 256 GB SSD disk. Used with a case.",
                  Price = 1200,
                  CreatedAt = DateTime.Now,
                  UpdatedAt = DateTime.Now,
                  StockCount = 1,
                  ImageUrl = "adv3.jpg",
                  Confirm = true,
                  CategoryId = 2,
                  UserId = 1

              },
              new AdvertEntity
              {
                  Id = 2,
                  Name = "MSI Notebook - I9+16 GB RAM",
                  Description = "It's enough. A notebook that is very clean, consumes very little, and runs very fast. Almost as cheap as water, practically free.",
                  Price = 1100,
                  CreatedAt = DateTime.Now,
                  UpdatedAt = DateTime.Now,
                  StockCount = 1,
                  ImageUrl = "adv4.jpg",
                  Confirm = true,
                  CategoryId = 2,
                  UserId = 1

              }
              );





        }



    }
}