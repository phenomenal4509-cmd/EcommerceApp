using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);
var connectionString =builder.Configuration.GetConnectionString("Ecommerce");

builder.Services.AddDbContext<AppDbContex>(options =>options.UseNpgsql(connectionString));
var app = builder.Build();
app.MapEndpoints();
app.Run();
