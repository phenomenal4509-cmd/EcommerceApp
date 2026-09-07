    using MyApp.Api.Models;
    using Microsoft.EntityFrameworkCore;
    namespace MyApp.Api.Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public static class DataExtensions
    {
        public static void MigrateDb(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContex>();
            dbContext.Database.Migrate();


            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            }
        }
        public static void AddAppStoreDb(this WebApplicationBuilder builder)
        {
            var connectionString =builder.Configuration.GetConnectionString("Ecommerce");

            builder.Services.AddDbContext<AppDbContex>(options => options.UseNpgsql(connectionString).UseSeeding((context, _) =>
        {
            foreach(var seedProduct in SeedData.Products)
            {
               var product = context.Set<Product>().FirstOrDefault(p => p.Id == seedProduct.Id);
               if(product is null)
                {
                    context.Set<Product>().Add(seedProduct);
                }
            }
            context.SaveChanges();

            // Reset the sequence so new inserts don't collide with seeded Ids
            context.Database.ExecuteSqlRaw("SELECT setval('\"Products_Id_seq\"', (SELECT MAX(\"Id\") FROM \"Products\"))");

        }).UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            foreach(var seedProduct in SeedData.Products)
            {
               var product = await context.Set<Product>().FirstOrDefaultAsync(p => p.Id == seedProduct.Id, cancellationToken);
               if(product is null)
                {
                     context.Set<Product>().Add(seedProduct);
                }
            }
            await context.SaveChangesAsync(cancellationToken);

            // Reset the sequence so new inserts don't collide with seeded Ids
            await context.Database.ExecuteSqlRawAsync("SELECT setval('\"Products_Id_seq\"', (SELECT MAX(\"Id\") FROM \"Products\"))", cancellationToken);
        }));
            
        }
        public static void AddIdentityContext(this WebApplicationBuilder builder){
        builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
        {
            options.Password.RequiredLength=8;
            options.User.RequireUniqueEmail=true;
        }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContex>();
        }
    }