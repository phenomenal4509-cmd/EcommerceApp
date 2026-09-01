using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyApp.Api.Data;
namespace MyApp.Api.EndPoints;
public static class TestEndPoints
{
    public static void MapTestEndPoints(this WebApplication app)
    {
        app.MapGet("/test", (HttpContext context) =>
        {
            var userId=(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var claim=context.User.FindFirstValue("AccountType");
            var name=context.User.FindFirstValue(ClaimTypes.Name);            
            var email=context.User.FindFirstValue(ClaimTypes.Email);
            
            return Results.Ok(
                new
                {
                    userId,
                    name,
                    email,
                    claim
                }
            );     
        }).RequireAuthorization();
        app.MapGet("/test/role",(HttpContext context)=>{
            var role=context.User.FindFirstValue(ClaimTypes.Role);
            return Results.Ok(role);
        }).RequireAuthorization();
        app.MapPost("/create/claim", async (HttpContext context,UserManager<ApplicationUser> userManager) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
            {
                return Results.BadRequest();
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return Results.NotFound();
            }

            var claim = new Claim("AccountType", "Premium");

            var result = await userManager.AddClaimAsync(user, claim);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok("Premium claim added");
        })
        .RequireAuthorization();
        app.MapDelete("/test/delete", async(UserManager<ApplicationUser>userManager,HttpContext context) =>
        {
            var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId is null) return Results.BadRequest("No user found");

            var user =await  userManager.FindByIdAsync(userId);

            if(user is null) return Results.BadRequest($"No user found with the Id {userId}");

            var claim = new Claim("AccountType", "Premium");

            var result= await userManager.RemoveClaimAsync(user,claim);

            return result.Succeeded?
            Results.NoContent():Results.BadRequest(result.Errors);
        }).RequireAuthorization();
        app.MapPost("/premium", () =>
        {
            return "Welcome Premium User";
        }).RequireAuthorization("PremiumAdmin");
    }
    
}