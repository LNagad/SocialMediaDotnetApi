using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Identity.Entities;

namespace SocialMedia.Infrastructure
{
  //Extension method - decorator
  public static class ServiceRegistration
  {
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration config)
    {
      #region Contexts
      if (config.GetValue<bool>("UseInMemoryDatabase"))
      {
        services.AddDbContext<IdentityContext>(opt => opt.UseInMemoryDatabase("IdentityDb"));
      }
      else
      {
        services.AddDbContext<IdentityContext>(opt =>
        {
          opt.EnableSensitiveDataLogging();
          opt.UseSqlServer(config.GetConnectionString("IdentityConnection"),
          m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
        });

      }
      #endregion

      #region Identity
      services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();

      services.AddAuthentication();
      #endregion

      return services;
    }
  }
}
