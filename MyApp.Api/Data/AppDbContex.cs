using Microsoft.EntityFrameworkCore;
using MyApp.Api.Models;

namespace MyApp.Api.Data;


public class AppDbContex(DbContextOptions<AppDbContex> options) :DbContext(options)
{
   public DbSet<Order> Orders => Set<Order>();
   public DbSet<Products> Products => Set<Products>();
   public DbSet<OrderItems> OrderItems => Set<OrderItems>();
   public DbSet<Tracking> Tracking => Set<Tracking>();
}