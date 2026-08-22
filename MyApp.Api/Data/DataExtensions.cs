using MyApp.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace MyApp.Api.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope= app.Services.CreateScope();
        var dbContext= scope.ServiceProvider.GetRequiredService<AppDbContex>();
        dbContext.Database.Migrate();
    }
    public static void AddAppStoreDb(this WebApplicationBuilder builder)
    {
        var connectionString =builder.Configuration.GetConnectionString("Ecommerce");

        builder.Services.AddDbContext<AppDbContex>(options => options.UseNpgsql(connectionString).UseSeeding((context, _)=>{
            if (!context.Set<Product>().Any())
            {
                context.Set<Product>().AddRange(
                SeedData.Products
                );
                context.SaveChanges();
            }
        }).UseAsyncSeeding(async(context, _,cancellationToken)=>{
            if (!await context.Set<Product>().AnyAsync(cancellationToken))
                {
                    context.Set<Product>().AddRange(SeedData.Products);
                    await context.SaveChangesAsync(cancellationToken);
                }
        }));
        
    }
}