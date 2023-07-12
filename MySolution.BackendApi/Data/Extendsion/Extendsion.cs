using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System;
using MySolution.BackendApi.Data.Entities;

namespace MySolution.BackendApi.Data.Extendsion
{
    public static class Extendsion
    {
        public static void Seeding(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Thịt"},
                new Category() { Id = 2, Name = "Hải Sản"},
                new Category() { Id = 3, Name = "Trứng"},
                new Category() { Id = 4, Name = "Đồ ăn nhanh"},
                new Category() { Id = 5, Name = "Sữa - Nước uống"},
                new Category() { Id = 6, Name = "Rau củ"},
                new Category() { Id = 7, Name = "Trái cây"}
                );

            modelBuilder.Entity<Product>().HasData(
                    new Product()
                    {
                        Id = 1,
                        CategoryId = 1,
                        Name = "Thịt ba rọi khay",
                        Price = 89300,
                        SeoAlias = "thit-ba-roi-khay",
                        Stock = 50,
                        Descrestion = "Thịt ba rọi khay",
                        IsFeatured = true,
                    }
                );

            var adminid = Guid.NewGuid().ToString();
            var roleid = "admin";
            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    UserName = "admin",
                    Id = adminid,
                    SecurityStamp = string.Empty,
                    Email = "khanhhuynh912@gmail.com",
                    FirstName = "Quoc Khanh",
                    LastName = "Huynh",
                    PasswordHash = hasher.HashPassword(null, "Admin@123"),
                    EmailConfirmed = true,
                    NormalizedUserName = "ADMIN"
                }
                );
            modelBuilder.Entity<Slide>().HasData(
                new Slide() { Id = 1, Image = "/img/categories/cat-1.0.jpg", Name = "Thịt", Url = "#" },
                new Slide() { Id = 2, Image = "/img/categories/cat-2.0.jpg", Name = "Hải sản", Url = "#" },
                new Slide() { Id = 3, Image = "/img/categories/cat-3.0.jpg", Name = "Trứng", Url = "#" },
                new Slide() { Id = 4, Image = "/img/categories/cat-4.0.jpg", Name = "Đồ ăn nhanh", Url = "#" },
                new Slide() { Id = 5, Image = "/img/categories/cat-5.0.jpg", Name = "Sữa - Nước uống", Url = "#" },
                new Slide() { Id = 6, Image = "/img/categories/cat-6.0.jpg", Name = "Rau củ", Url = "#" },
                new Slide() { Id = 7, Image = "/img/categories/cat-7.0.jpg", Name = "Trái cây", Url = "#" }

                );
        }
    }
}
