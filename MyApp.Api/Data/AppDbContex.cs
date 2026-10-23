using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApp.Api.Models;

namespace MyApp.Api.Data;

public class AppDbContex(DbContextOptions<AppDbContex> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Cart> Cart => Set<Cart>();
    public DbSet<CartItem> CartItem => Set<CartItem>();
    public DbSet<Tracking> Tracking => Set<Tracking>();
    public DbSet<DeliveryOption> DeliveryOptions => Set<DeliveryOption>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>()
        .Property(p=>p.Version)
        .IsConcurrencyToken();
        modelBuilder.Entity<OrderItem>()
            .HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tracking>()
            .HasOne<Order>()
            .WithMany(o => o.TrackingHistory)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Referential integrity for columns that had indexes but no FK constraints.
        // OrderItem.ProductId intentionally has NO FK so order history survives product deletion
        // (order details fall back to "Unknown product" when the product row is gone).

        modelBuilder.Entity<Cart>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(c => c.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CartItem>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne<DeliveryOption>()
            .WithMany()
            .HasForeignKey(i => i.DeliveryOptionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne<DeliveryOption>()
            .WithMany()
            .HasForeignKey(i => i.DeliveryOptionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.HasPostgresExtension("pg_trgm");

        // Products: sorting (ORDER BY Price/Name + Id tiebreaker) and price range filters.
        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.Price, p.Id })
            .HasDatabaseName("IX_Products_Price_Id");

        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.Name, p.Id })
            .HasDatabaseName("IX_Products_Name_Id");

        // Products: substring search (ILIKE '%term%') uses trigram indexes.
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name)
            .HasMethod("gin")
            .HasOperators("gin_trgm_ops")
            .HasDatabaseName("IX_Products_Name_Trim");

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Description)
            .HasMethod("gin")
            .HasOperators("gin_trgm_ops")
            .HasDatabaseName("IX_Products_Description_Trim");

        modelBuilder.Entity<Cart>()
            .HasIndex(c => c.UserID)
            .IsUnique()
            .HasDatabaseName("IX_Cart_UserID");

        // Orders: loaded per user, newest id first.
        modelBuilder.Entity<Order>()
            .HasIndex(o => new { o.UserId, o.Id })
            .HasDatabaseName("IX_Orders_UserId_Id");

        // Orders: filtering by status and date-range reporting.
        modelBuilder.Entity<Order>()
            .HasIndex(o => o.CurrentStatus)
            .HasDatabaseName("IX_Orders_CurrentStatus");

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.OrderDate)
            .HasDatabaseName("IX_Orders_OrderDate");

        // Tracking: sorted by time when showing status history.
        modelBuilder.Entity<Tracking>()
            .HasIndex(t => t.StatusChangedAt)
            .HasDatabaseName("IX_Tracking_StatusChangedAt");

        modelBuilder.Entity<CartItem>()
            .HasIndex(i => i.ProductId)
            .HasDatabaseName("IX_CartItem_ProductId");

        modelBuilder.Entity<CartItem>()
            .HasIndex(i => i.DeliveryOptionId)
            .HasDatabaseName("IX_CartItem_DeliveryOptionId");

        modelBuilder.Entity<OrderItem>()
            .HasIndex(i => i.ProductId)
            .HasDatabaseName("IX_OrderItems_ProductId");

        modelBuilder.Entity<OrderItem>()
            .HasIndex(i => i.DeliveryOptionId)
            .HasDatabaseName("IX_OrderItems_DeliveryOptionId");
    }
}
