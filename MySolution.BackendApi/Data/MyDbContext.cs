using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MySolution.BackendApi.Data.Entities;
using MySolution.Data.Enums;
using MySolution.BackendApi.Data.Extendsion;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MySolution.BackendApi.Data
{
    public class MyDbContext : IdentityDbContext<User,IdentityRole,string>
    {
        public MyDbContext(DbContextOptions options) : base(options) {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(x => x.UserId);

            modelBuilder.Entity<Product>().Property(x => x.DateCreated).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Product>().Property(x => x.ViewCount).HasDefaultValue(0);
            modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);

            modelBuilder.Entity<ProductImage>().Property(x => x.IsDefault).HasDefaultValue(false);
            modelBuilder.Entity<ProductImage>().Property(x => x.DateCeated).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<ProductImage>().HasOne(x => x.Product).WithMany(x => x.ProductImages).HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<Order>().Property(x => x.OrderDate).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Order>().Property(x => x.Status).HasDefaultValue(OrderStatus.Progress);
            modelBuilder.Entity<Order>().HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<OrderDetail>().HasOne(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId);
            modelBuilder.Entity<OrderDetail>().HasOne(x => x.Product).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductId);
            modelBuilder.Entity<OrderDetail>().HasKey(x => new { x.OrderId, x.ProductId }) ;

            modelBuilder.Seeding();
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Slide> Slides { get; set; }
    }
}
