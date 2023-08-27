using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core;
using SocialMedia.Infrastructure;
using SocialMedia.Infrastructure.Identity.Entities;
using SocialMedia.Infrastructure.Identity.Seeds;
using SocialMediaApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
  opt.Filters.Add(new ProducesAttribute("application/json"));
}).ConfigureApiBehaviorOptions(opt =>
{
  // not assume any sources for parameters // use the documentation speficied for us
  opt.SuppressInferBindingSourcesForParameters = true; 
  // supress automatic error response
  opt.SuppressMapClientErrors = true;
});
//ignore null
//.AddNewtonsoftJson(opt => opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();

// Adding the dependency injection layers
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructureForApi(builder.Configuration);
builder.Services.AddServicesInfrastructure();

builder.Services.AddSwaggerExtension();
builder.Services.AddApiVersioningExtension();

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


app.Use(async (context, next) =>
{
  if (context.Request.Path == "/")
  {
    context.Response.Redirect("/swagger"); // Cambia "tu-ruta-destino" por la URL a la que deseas redirigir
    return;
  }

  await next();
});

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerExtension();
app.UseErrorHandlerMiddleware();

app.UseHsts();

app.UseHealthChecks("/health");

app.MapControllers();

app.Run();
