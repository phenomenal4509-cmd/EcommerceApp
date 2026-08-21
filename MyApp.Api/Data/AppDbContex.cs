using Microsoft.EntityFrameworkCore;
using MyApp.Api.Models;

namespace MyApp.Api.Data;


public class AppDbContex(DbContextOptions<AppDbContex> options) :DbContext(options)
{
   public DbSet<Order> Orders => Set<Order>();
   public DbSet<Product> Products => Set<Product>();
   public DbSet<OrderItems> OrderItems => Set<OrderItems>();
   public DbSet<Tracking> Tracking => Set<Tracking>();
}