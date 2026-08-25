using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.Dto;
using MyApp.Api.Models;

namespace MyApp.Api.EndPoints;

public static class TrackingEndPoints
{
    public static void MapTrackingEndPoints(this WebApplication app)
    {
        app.MapGet("/orders/{id}/tracking",(int id,AppDbContex dbContext) =>
        {
            var order=dbContext.Orders.Include(o=>o.TrackingHistory).FirstOrDefault(o=>o.Id==id);
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
        });
        app.MapPut("/orders/{id}/tracking", (int id , AppDbContex dbContext,UpdateTrackingDto updateTracking) =>
        {
            var order=dbContext.Orders.Include(o=>o.TrackingHistory).FirstOrDefault(o=>o.Id==id);
            if(order is null)
            {
                return Results.NotFound("No such order exists");
            }
            Tracking newTrack = new()
            {
                OrderId=order.Id,
                Status=updateTracking.Status,
                StatusChangedAt=DateTimeOffset.Now
            };
            order.TrackingHistory.Add(newTrack);
            dbContext.SaveChanges();
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
        });
        
    }
}