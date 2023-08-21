using SocialMedia.Core;
using SocialMedia.Infrastructure;
using SocialMediaApi.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
  .AddControllers(opt => opt.Filters.Add<GlobalExceptionFilter>())
  //ignore null
  .AddNewtonsoftJson(opt => opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);

// Adding the dependency injection layers
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddServicesLayer();

// Adding Swagger
builder.Services.AddSwagger($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

// Ading JWT
builder.Services.AddJWTAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//  app.UseSwagger();
//  app.UseSwaggerUI(options =>
//  {
//    options.SwaggerEndpoint("../swagger/v1/swagger.json", "Social Media API v1");
//    options.RoutePrefix = "swagger";
//  });
//}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
  options.SwaggerEndpoint("../swagger/v1/swagger.json", "Social Media API v1");
  options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

//app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
