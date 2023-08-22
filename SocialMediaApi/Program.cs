using Microsoft.AspNetCore.Identity;
using SocialMedia.Core;
using SocialMedia.Infrastructure;
using SocialMedia.Infrastructure.Identity.Entities;
using SocialMedia.Infrastructure.Identity.Seeds;
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

builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddServicesInfrastructure();

builder.Services.AddSwaggerExtension();
builder.Services.AddApiVersioningExtension();

builder.Services.AddJWTAuthentication(builder.Configuration);

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  try
  {
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await DefaultRoles.SeedAsync(userManager, roleManager);
    await DefaultAdminUser.SeedAsync(userManager, roleManager);
    await DefaultBasicUser.SeedAsync(userManager, roleManager);
  }
  catch (Exception ex)
  {
    Console.WriteLine(ex.Message);
  }
}



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
