using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApp.Api.Data;
using MyApp.Api.Dto;

namespace MyApp.APi.EndPoints;

public static class UserEndPoints
{
    public static void MapUserEndPoints(this WebApplication app)
    {

        app.MapPost("/users/register", async (RegistrationDto request, UserManager<ApplicationUser> userManager) =>
        {
            var user = new ApplicationUser
            {
                UserName  = request.Email,
                Email     = request.Email,
                FirstName = request.FirstName,
                LastName  = request.LastName
            };
            var result = await userManager.CreateAsync(user, request.Password);
            return result.Succeeded
                ? Results.Ok("User created.")
                : Results.BadRequest(result.Errors);
        });



        app.MapGet("/users/me", (HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var name   = context.User.FindFirstValue(ClaimTypes.Name);
            var email  = context.User.FindFirstValue(ClaimTypes.Email);
            var claim  = context.User.FindFirstValue("AccountType");

            return Results.Ok(new { userId, name, email, accountType = claim });
        }).RequireAuthorization();



        app.MapPut("/users/me", async (HttpContext context,
            [FromServices] UserManager<ApplicationUser> userManager,
            UpdateUserDto request) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return Results.NotFound("User could not be found.");

            user.FirstName = request.FirstName;
            user.LastName  = request.LastName;

            var result = await userManager.UpdateAsync(user);
            return result.Succeeded
                ? Results.Ok()
                : Results.BadRequest(result.Errors);
        }).RequireAuthorization();



        app.MapPost("/users/2fA", async (HttpContext context,
            UserManager<ApplicationUser> userManager) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return Results.NotFound("User could not be found.");

            var key = await userManager.GetAuthenticatorKeyAsync(user);
            if (key is null)
            {
                await userManager.ResetAuthenticatorKeyAsync(user);
                key = await userManager.GetAuthenticatorKeyAsync(user);
            }

            return Results.Ok(new { email = user.Email, sharedKey = key });
        }).RequireAuthorization();
    }
}