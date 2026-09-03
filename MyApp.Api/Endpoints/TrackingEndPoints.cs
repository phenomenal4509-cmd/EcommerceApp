using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.Dto;
using MyApp.Api.Models;

namespace MyApp.Api.EndPoints;

public static class TrackingEndPoints
{
    public static void MapTrackingEndPoints(this WebApplication app)
    {
        app.MapGet("/orders/{id}/tracking", async (int id, AppDbContex dbContext, HttpContext context) =>
        {
            var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId is null) return Results.Unauthorized();
            var order = await dbContext.Orders.Include(o=>o.TrackingHistory).FirstOrDefaultAsync(o=>o.Id==id && userId==o.UserId);
            if(order is null)
            {
                return Results.NotFound("No such order exists");
            }
            ICollection<TrackingHistoryDto>trackingHistory=new List<TrackingHistoryDto>();
            foreach(var element in order.TrackingHistory)
            {
            TrackingHistoryDto tracking=new(
                element.Status,
                element.StatusChangedAt
            );
            trackingHistory.Add(tracking);              
            }
            TrackingDto sendHistory= new(order.Id,order.CurrentStatus,trackingHistory);

            return Results.Ok(sendHistory);
        }).RequireAuthorization();
        app.MapPost("/orders/{id}/tracking", async (int id, AppDbContex dbContext, UpdateTrackingDto updateTracking) =>
        {
            var order = await dbContext.Orders.Include(o => o.TrackingHistory).FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
            {
                return Results.NotFound("No such order exists");
            }

            // Define which statuses each status can transition to
            var allowedTransitions = new Dictionary<string, HashSet<string>>
            {
                ["Pending"]    = ["Processing", "Cancelled"],
                ["Processing"] = ["Shipped",     "Cancelled"],
                ["Shipped"]    = ["Delivered"],
                ["Delivered"]  = [],          // terminal — no further transitions
                ["Cancelled"]  = [],          // terminal — no further transitions
            };

            var currentStatus = order.CurrentStatus;
            var newStatus     = updateTracking.Status;

            if (!allowedTransitions.ContainsKey(currentStatus))
            {
                return Results.BadRequest($"Unknown current status '{currentStatus}'.");
            }

            if (!allowedTransitions[currentStatus].Contains(newStatus))
            {
                var allowed = allowedTransitions[currentStatus];
                var allowedStr = allowed.Any() ? string.Join(", ", allowed) : "none (terminal status)";
                return Results.BadRequest($"Cannot transition from '{currentStatus}' to '{newStatus}'. " +$"Allowed transitions: {allowedStr}.");
            }
            
            Tracking newTrack = new()
            {
                OrderId = order.Id,
                Status = newStatus,
                StatusChangedAt = DateTimeOffset.UtcNow
            };

            order.CurrentStatus = newStatus;
            dbContext.Tracking.Add(newTrack);
            await dbContext.SaveChangesAsync();

            return Results.Ok();
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}