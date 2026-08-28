using System.Security.Claims;

namespace MyApp.Api.EndPoints;
public static class TestEndPoints
{
    public static void MapTestEndPoints(this WebApplication app)
    {
        app.MapGet("/test", (HttpContext context) =>
        {
            var userId=(context.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var name=context.User.FindFirstValue(ClaimTypes.Name);            
            var email=context.User.FindFirstValue(ClaimTypes.Email);  
            return Results.Ok(
                new
                {
                    userId,
                    name,
                    email
                }
            );     
        }).RequireAuthorization();       
    }
    
}