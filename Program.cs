using foodies_yelp;
using foodies_yelp.Data;
using foodies_yelp.Endpoints;
using foodies_yelp.Services;
using Microsoft.EntityFrameworkCore;
using foodies_yelp.Models.Options;
using foodies_yelp.Profiles.RestaurantProfile;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);
var httpContextAccessor = new HttpContextAccessor();
var context = httpContextAccessor.HttpContext;

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddAutoMapper(typeof(RestaurantProfile), typeof(ReviewProfile));

// Configure services
builder.Services.AddHttpClient("YelpService", client => 
{
    client.BaseAddress = new Uri(configuration.GetValue<string>(YelpConstants.BaseAddress));    
});
builder.Services.Configure<YelpOptions>(builder.Configuration.GetSection(YelpConstants.SectionName));

// Add YelpService as a singleton with configuration
builder.Services.AddSingleton<YelpService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization();

var app = builder.Build();

app.ConfigurationHealthCheckEndpoints();
app.ConfigurationRestaurantEndpoints();
app.ConfigurationReviewEndpoints();

app.Run();