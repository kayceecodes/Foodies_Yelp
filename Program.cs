using foodies_yelp;
using foodies_yelp.Data;
using foodies_yelp.Endpoints;
using foodies_yelp.Services;
using Microsoft.EntityFrameworkCore;
using foodies_yelp.Profiles.BusinessProfile;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddAutoMapper(typeof(BusinessProfile), typeof(ReviewProfile));

// Configure services
builder.Services.AddHttpClient("YelpService", client => 
{
    client.BaseAddress = new Uri(configuration.GetValue<string>(YelpConstants.BaseAddress));    
});

// Add YelpService as a singleton with configuration
builder.Services.AddSingleton<YelpService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization();

var app = builder.Build();

app.ConfigurationHealthCheckEndpoints();
app.ConfigurationBusinessEndpoints();
app.ConfigurationReviewEndpoints();

app.Run();