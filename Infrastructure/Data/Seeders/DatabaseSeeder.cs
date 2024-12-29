using EcomSiteMVC.Core.Enums;
using EcomSiteMVC.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomSiteMVC.Infrastructure.Data.Seeders
{
    public class DatabaseSeeder
    {
        public static void SeedAdminUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                Username = "superadmin",
                Email = "superadmin@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Superadmin@123"), // Ensure this is a hashed password
                Role = Role.Superadmin, // Adjust based on your enum
                IsActive = true,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now)
            });
        }

        public static void SeedEntityRelationship(ModelBuilder modelBuilder)
        {
            // Fluent API configuration
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserProfile)
                .WithOne(up => up.User)
                .HasForeignKey<UserProfile>(up => up.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.Customer)
                .HasForeignKey<Cart>(c => c.CustomerId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.CartItems)
                .WithOne(ci => ci.Product);
        }
    }
}
