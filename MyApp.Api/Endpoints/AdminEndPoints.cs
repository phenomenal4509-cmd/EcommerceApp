using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApp.Api.Dto;
using MyApp.Api.Data;
using System.Security.Claims;
namespace MyApp.Api.EndPoints;

public static class AdminEndPoints
{
    public static void MapAdminEndPoints(this WebApplication app)
    {
        app.MapGet("/admin/users/{id}/roles",async(string id, UserManager<ApplicationUser> userManager) =>
        {
            var user=await userManager.FindByIdAsync(id);
            if (user is null)
            {
                return Results.NotFound($"No such user exist with the id {id}");
            }
            var result= await userManager.GetRolesAsync(user);
            return Results.Ok(result.Select(r => new RoleDto(r)).ToList());
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapPost("/admin/users/{id}/roles", async(string id, [FromServices] UserManager<ApplicationUser> userManager, AddRoleDto request) =>
        {
            var user=await userManager.FindByIdAsync(id);
            if (user is null)
            {
                return Results.NotFound($"No such user exist with the id {id}");
            }
            var result=await userManager.AddToRoleAsync(user ,request.role);
            return result.Succeeded
            ? Results.Ok("Role has been added")
            : Results.BadRequest(result.Errors);
        }).RequireAuthorization();
        app.MapDelete("/admin/users/{id}/roles/{role}",async(string role,string id ,UserManager<ApplicationUser> userManager) =>
        {
            var user=await userManager.FindByIdAsync(id);
            if(user is null)
            {
                return Results.NotFound($"No such user exist with the id {id}");
            }
            var result = await userManager.RemoveFromRoleAsync(user, role);

            return result.Succeeded
            ? Results.Ok("Role removed")
            : Results.BadRequest(result.Errors);

        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapDelete("/admin/users/{id}/roles",async (string id, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
                return Results.NotFound($"No such user exists with the id {id}");

            var roles = await userManager.GetRolesAsync(user);

            var result = await userManager.RemoveFromRolesAsync(user, roles);

            return result.Succeeded
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"));
        app.MapGet("/admin/users/{id}/claims",async(string id, UserManager<ApplicationUser> userManager) =>
        {
            var user=await userManager.FindByIdAsync(id);
            if (user is null)
            {
                return Results.NotFound($"No such user exist with the id {id}");
            }
            var result= await userManager.GetClaimsAsync(user);
            return Results.Ok(result.Select(c => new ClaimDto(c.Type,c.Value)).ToList());
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapPost("/admin/users/{id}/claims",async(string id, [FromServices] UserManager<ApplicationUser> userManager, AddClaimDto request)=>
        {
           var user=await userManager.FindByIdAsync(id);
           if(user is null) return Results.NotFound($"No such user exist with the id {id}");
           var result=await userManager.AddClaimAsync(user, new Claim(request.Type,request.Value));
            return result.Succeeded
            ? Results.Ok("Claim added")
            : Results.BadRequest(result.Errors);
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapDelete("/admin/users/{id}/claim/{claimType}/{claimValue}",async (string id, string claimType, string claimValue, [FromServices] UserManager<ApplicationUser> userManager) =>
        {
            var user=await userManager.FindByIdAsync(id);
            if(user is null)
            {
                return Results.NotFound($"No such user exist with the id {id}");
            }
            var claim = new Claim(claimType, claimValue);
            var result = await userManager.RemoveClaimAsync(user, claim);

            return result.Succeeded
            ? Results.Ok("Claim removed")
            : Results.BadRequest(result.Errors);

        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapDelete("/admin/users/{id}/claims",
            async (string id, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);

            if (user is null)
                return Results.NotFound($"No such user exists with the id {id}");

            var claims = await userManager.GetClaimsAsync(user);

            var result = await userManager.RemoveClaimsAsync(user, claims);

            return result.Succeeded
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        })
        .RequireAuthorization(policy => policy.RequireRole("Admin"));
        app.MapGet("/me", async (ClaimsPrincipal principal,UserManager<ApplicationUser> userManager) =>
        {
            var userId = userManager.GetUserId(principal);

            if (userId is null)
                return Results.Unauthorized();

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
                return Results.NotFound("No such user found");

            return Results.Ok(new
            {
                id = user.Id,
                email = user.Email
            });
        })
        .RequireAuthorization();
    }
}