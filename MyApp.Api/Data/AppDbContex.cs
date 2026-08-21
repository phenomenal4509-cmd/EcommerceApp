using Microsoft.EntityFrameworkCore;
using MyApp.Api.Models;

namespace MyApp.Api.Data;


public class AppDbContex(DbContextOptions<AppDbContex> options) :DbContext(options)
{
   public DbSet<Product> Products => Set<Product>();
}