namespace MyApp.Api.EndPoints;

public static class EndPointExtensions
{
    public static void MapAllEndpoints(this WebApplication app)
    {
        app.MapProductEndpoints();
        app.MapOrderEndPoints();
        app.MapCartEndPoints();
        app.MapTrackingEndPoints();
        app.MapTestEndPoints();
        app.MapRoleEndPoints();
    }
}