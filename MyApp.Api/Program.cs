using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.EndPoints;
using MyApp.Api.Exceptions;
using Microsoft.AspNetCore.Authentication.BearerToken;
var builder = WebApplication.CreateBuilder(args);

builder.AddAppStoreDb();
builder.AddIdentityContext();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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
builder.Services.Configure<BearerTokenOptions>((options) =>
{
    options.BearerTokenExpiration=TimeSpan.FromHours(2);// this changes the actual expiration of the bearer token , the minimum it is the better but the decision should be intellectual
});
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseExceptionHandler();
app.MapIdentityApi<ApplicationUser>();

app.MigrateDb();
app.MapAllEndpoints();

app.Run();