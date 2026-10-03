using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.EndPoints;
using MyApp.Api.Exceptions;
using MyApp.Api.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
var builder = WebApplication.CreateBuilder(args);

builder.AddAppStoreDb();
builder.AddIdentityContext();
builder.Services.AddTransient<IEmailSender<ApplicationUser>, EmailSender>();
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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<ApplicationUser>();

app.MigrateDb();
app.MapAllEndpoints();

app.Run();