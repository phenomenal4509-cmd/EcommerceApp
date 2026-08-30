using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MyApp.Api.EndPoints;

public static class RoleEndPoints
{
    public static void MapRoleEndPoints(this WebApplication app)
    {
        app.MapPost("/create/role", async(RoleManager<IdentityRole> roleManager) =>
        {
            var role= new IdentityRole("Admin");
            var result= await roleManager.CreateAsync(role);
            return result.Succeeded
            ? Results.Ok("Role created")
            : Results.BadRequest(result.Errors);
        });
        app.MapPost("/upgrade/role/admin", async(UserManager<IdentityUser> userManager,HttpContext context)  =>
        {
            var userEmail=context.User.FindFirstValue(ClaimTypes.Email);
            if(userEmail is null) return Results.BadRequest("Could not process user's mail");
            var user=await userManager.FindByEmailAsync(userEmail);
            if(user is null)
            {
                return Results.NotFound("User does not exist");
            }
            var result=await userManager.AddToRoleAsync(user,"Admin");
            return result.Succeeded
            ? Results.Ok("User has been made an Admin")
            : Results.BadRequest(result.Errors);
        });
        
    }
}