using foodies_yelp;
using foodies_yelp.Data;
using foodies_yelp.Endpoints;
using foodies_yelp.Services;
using Microsoft.EntityFrameworkCore;
using foodies_yelp.Models.Options;
using foodies_yelp.Profiles.RestaurantProfile;

var builder = WebApplication.CreateBuilder(args);
var httpContextAccessor = new HttpContextAccessor();
var context = httpContextAccessor.HttpContext;

ConfigurationManager configuration = builder.Configuration;

//  // Load configuration from appsettings.json
// temporarily moved to check for user secrets //builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddAutoMapper(typeof(RestaurantProfile));

// Configure services
builder.Services.AddHttpClient("YelpService", client => 
{
    client.BaseAddress = new Uri(configuration.GetValue<string>(YelpConstants.BaseAddress));    
});
builder.Services.Configure<Yelp>(builder.Configuration.GetSection(YelpConstants.SectionName));

// Add YelpService as a singleton with configuration
builder.Services.AddSingleton<YelpService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: MyAllowSpecificOrigins,
//                       policy  =>
//                       {
//                           policy.WithOrigins("http://example.com",
//                                               "http://www.contoso.com");
//                       });
// });

// app.MapGroup("Some Name - Auth Endpoints").AddEndpointFilter<ApiKeyEndpointFilter>();

app.ConfigurationRestaurantEndpoints();
app.ConfigurationReviewEndpoints();

app.Run();
    