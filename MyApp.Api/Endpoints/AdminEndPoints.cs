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

        app.MapGet("/admin/users/{id}/roles", async (string id, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
                return Results.NotFound($"No user exists with id '{id}'.");

            var roles = await userManager.GetRolesAsync(user);
            return Results.Ok(roles.Select(r => new RoleDto(r)).ToList());
        }).RequireAuthorization(p => p.RequireRole("Admin"));



        app.MapPost("/admin/users/{id}/roles", async (
            string id,
            [FromServices] UserManager<ApplicationUser> userManager,
            AddRoleDto request) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
                return Results.NotFound($"No user exists with id '{id}'.");

            var result = await userManager.AddToRoleAsync(user, request.role);
            return result.Succeeded
                ? Results.Ok("Role added.")
                : Results.BadRequest(result.Errors);
        }).RequireAuthorization(p => p.RequireRole("Admin"));



        app.MapDelete("/admin/users/{id}/roles/{role}", async (
            string id, string role,
            UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
                return Results.NotFound($"No user exists with id '{id}'.");

            var result = await userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        }).RequireAuthorization(p => p.RequireRole("Admin"));



        app.MapDelete("/admin/users/{id}/roles", async (string id, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
                return Results.NotFound($"No user exists with id '{id}'.");

            var roles  = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            return result.Succeeded
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        }).RequireAuthorization(p => p.RequireRole("Admin"));



        app.MapGet("/admin/users/{id}/claims", async (string id, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
                return Results.NotFound($"No user exists with id '{id}'.");

            var claims = await userManager.GetClaimsAsync(user);
            return Results.Ok(claims.Select(c => new ClaimDto(c.Type, c.Value)).ToList());
        }).RequireAuthorization(p => p.RequireRole("Admin"));



        app.MapPost("/admin/users/{id}/claims", async (
            string id,
            [FromServices] UserManager<ApplicationUser> userManager,
            AddClaimDto request) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
                return Results.NotFound($"No user exists with id '{id}'.");

            var result = await userManager.AddClaimAsync(user, new Claim(request.Type, request.Value));
            return result.Succeeded
                ? Results.Ok("Claim added.")
                : Results.BadRequest(result.Errors);
        }).RequireAuthorization(p => p.RequireRole("Admin"));



        app.MapDelete("/admin/users/{id}/claim/{claimType}/{claimValue}", async (
            string id, string claimType, string claimValue,
            [FromServices] UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
                return Results.NotFound($"No user exists with id '{id}'.");

            var result = await userManager.RemoveClaimAsync(user, new Claim(claimType, claimValue));
            return result.Succeeded
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        }).RequireAuthorization(p => p.RequireRole("Admin"));



        app.MapDelete("/admin/users/{id}/claims", async (string id, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(id);
            if (user is null)
                return Results.NotFound($"No user exists with id '{id}'.");

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);
            return result.Succeeded
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        }).RequireAuthorization(p => p.RequireRole("Admin"));
    }
}