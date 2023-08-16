using SocialMedia.Core;
using SocialMedia.Infrastructure;
using SocialMedia.Infrastructure.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers( opt => opt.Filters.Add<GlobalExceptionFilter>() );
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
