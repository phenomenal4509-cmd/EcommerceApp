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

                foreach(var seedProduct in SeedData.Products)
                {
                   var product=context.Set<Product>().FirstOrDefault(p=>p.Id==seedProduct.Id);
                   if(product is null)
                    {
                        context.Set<Product>().Add(seedProduct);
                    }
                }
                    context.SaveChanges();

            }).UseAsyncSeeding(async(context, _,cancellationToken)=>{
                foreach( var seedProduct in SeedData.Products)
                {
                   var product=await context.Set<Product>().FirstOrDefaultAsync(p=>p.Id==seedProduct.Id,cancellationToken);
                   if(product is null)
                    {
                         context.Set<Product>().Add(seedProduct);
                    }
                }
                    await context.SaveChangesAsync(cancellationToken);
            }));
            
        }
    }