using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppStoreDb();
var app = builder.Build();
app.MigrateDb();
app.MapEndpoints();
app.Run();
