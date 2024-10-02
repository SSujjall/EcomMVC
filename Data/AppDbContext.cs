using EcomSiteMVC.Models.Entities;
using EcomSiteMVC.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace EcomSiteMVC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
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

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product);

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

            modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), // Ensure this is a hashed password
                Role = Role.Admin, // Adjust based on your enum
                IsActive = true,
                CreatedDate = DateTime.Now
            }
    );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
