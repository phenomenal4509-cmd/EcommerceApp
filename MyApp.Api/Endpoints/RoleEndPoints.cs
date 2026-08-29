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
        app.MapPost("/create/role/make-admin", async(UserManager<IdentityUser> userManager)  =>
        {
            var user=await userManager.FindByEmailAsync("ibrahim1234@gmail.com");
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