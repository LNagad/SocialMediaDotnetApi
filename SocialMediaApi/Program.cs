using SocialMedia.Core;
using SocialMedia.Infrastructure;
using SocialMedia.Infrastructure.Services;
using SocialMediaApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
  .AddControllers( opt => opt.Filters.Add<GlobalExceptionFilter>() )              
  //ignore null
  .AddNewtonsoftJson( opt => opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);
  //.ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding the dependency injection layers
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddServicesLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseRouting();

app.MapControllers();

app.Run();
