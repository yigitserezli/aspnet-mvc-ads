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

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AdvertEntity> Adverts { get; set; }
        public DbSet<AdvertCommentsEntity> AdvertComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdvertCommentsEntity>().HasOne(c => c.User).WithMany(z => z.AdvertComments).HasForeignKey(c=>c.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<AdvertCommentsEntity>().HasOne(c => c.Advert).WithMany(z => z.AdvertComments).HasForeignKey(c => c.AdvertId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<CustomerFavListentity>().HasOne(c => c.User).WithMany(z => z.CustomerFavs).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction);



        }



    }
}
