using SocialMedia.Core;
using SocialMedia.Infrastructure;
using SocialMediaApi.Extensions;
using SocialMediaApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
  .AddControllers(opt => opt.Filters.Add<GlobalExceptionFilter>())
  //ignore null
  .AddNewtonsoftJson(opt => opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();

// Adding the dependency injection layers
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddServicesLayer();

builder.Services.AddSwaggerExtension();
builder.Services.AddApiVersioningExtension();

builder.Services.AddJWTAuthentication(builder.Configuration);

var app = builder.Build();

app.UseSwaggerExtension();

app.UseExceptionHandler("/Error");
app.UseHsts();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseHealthChecks("/health");

app.MapControllers();

app.Run();
