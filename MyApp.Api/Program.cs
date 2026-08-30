using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppStoreDb();
builder.AddIdentityContext();

builder.Services.AddAuthorization((options) =>
{
    options.AddPolicy("PremiumUser", (policy) =>
    {
        policy.RequireClaim("AccountType","Premium");
    });
    options.AddPolicy("PremiumAdmin", (policy) =>
    {
        policy.RequireClaim("AccountType","Premium");
        policy.RequireRole("Admin");
    });
});

var app = builder.Build();

app.MapIdentityApi<ApplicationUser>();

app.MigrateDb();
app.MapAllEndpoints();

app.Run();