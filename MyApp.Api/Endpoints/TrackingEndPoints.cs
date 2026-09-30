using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.Dto;
using MyApp.Api.Models;

namespace MyApp.Api.EndPoints;

public static class TrackingEndPoints
{
    private static readonly Dictionary<string, HashSet<string>> AllowedTransitions = new()
    {
        ["Pending"]    = ["Processing", "Cancelled"],
        ["Processing"] = ["Shipped",    "Cancelled"],
        ["Shipped"]    = ["Delivered"],
        ["Delivered"]  = [],
        ["Cancelled"]  = [],
    };

    public static void MapTrackingEndPoints(this WebApplication app)
    {

        app.MapGet("/orders/{id}/tracking", async (int id, AppDbContex dbContext, HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var order = await dbContext.Orders
                .Include(o => o.TrackingHistory)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order is null)
                return Results.NotFound("No such order exists.");

            var trackingHistory = order.TrackingHistory
                .Select(e => new TrackingHistoryDto(e.Status, e.StatusChangedAt))
                .ToList();

            return Results.Ok(new TrackingDto(order.Id, order.CurrentStatus, trackingHistory));
        }).RequireAuthorization();



        app.MapPost("/orders/{id}/tracking", async (int id, AppDbContex dbContext, UpdateTrackingDto updateTracking) =>
        {
            var order = await dbContext.Orders
                .Include(o => o.TrackingHistory)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return Results.NotFound("No such order exists.");

            var currentStatus = order.CurrentStatus;
            var newStatus     = updateTracking.Status;

            if (!AllowedTransitions.TryGetValue(currentStatus, out var validNext))
                return Results.BadRequest($"Unknown current status '{currentStatus}'.");

            if (!validNext.Contains(newStatus))
            {
                var allowedStr = validNext.Any() ? string.Join(", ", validNext) : "none (terminal status)";
                return Results.BadRequest(
                    $"Cannot transition from '{currentStatus}' to '{newStatus}'. Allowed: {allowedStr}.");
            }

            order.CurrentStatus = newStatus;
            dbContext.Tracking.Add(new Tracking
            {
                OrderId         = order.Id,
                Status          = newStatus,
                StatusChangedAt = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync();

            return Results.Ok();
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}