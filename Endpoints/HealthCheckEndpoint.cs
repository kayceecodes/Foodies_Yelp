namespace foodies_yelp.Endpoints;

public static class HealthCheckEndpoints
{
    public static void ConfigurationHealthCheckEndpoints(this WebApplication app) 
    {
        app.MapGet("/api/healthcheck", () =>
        {
            Console.WriteLine("Health Check called");
            return TypedResults.Ok("Health Check OK!");
        }).WithName("HealthCheck");
    }
}
 