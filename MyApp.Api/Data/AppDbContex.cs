using Microsoft.EntityFrameworkCore;
using MyApp.Api.Models;

namespace MyApp.Api.Data;


public class AppDbContex(DbContextOptions<AppDbContex> options) :DbContext(options)
{
   public DbSet<Product> Products => Set<Product>();
   public DbSet<Order> Orders=>Set<Order>();
   public DbSet<OrderItem> OrderItems => Set<OrderItem>();
   public DbSet<Cart> Cart=>Set<Cart>();
   public DbSet<CartItem> CartItem=>Set<CartItem>();
   protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<OrderItem>()
        .HasOne<Order>()
        .WithMany(o => o.OrderItems)
        .HasForeignKey(oi => oi.OrderId)
        .OnDelete(DeleteBehavior.Cascade);
}
}
 