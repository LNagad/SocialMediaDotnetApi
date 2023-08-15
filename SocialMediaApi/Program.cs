using SocialMedia.Infrastructure;
using SocialMedia.Core;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
  //.ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding the dependency injection layers
builder.Services.AddInfrastructure(builder.Configuration);
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

app.MapControllers();

app.Run();
