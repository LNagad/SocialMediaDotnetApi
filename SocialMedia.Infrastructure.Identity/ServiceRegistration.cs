using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Core.Interfaces.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Identity.Entities;
using SocialMedia.Infrastructure.Identity.Services;

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
      //services.ConfigureApplicationCookie( opt =>
      //{
      //  opt.LoginPath = "/Account/Login";
      //  opt.AccessDeniedPath = "/Account/AccessDenied";
      //});
      services.AddAuthentication();
      #endregion

      #region Services
      services.AddTransient<IAccountService, AccountService>();
      #endregion
      return services;
    }
  }
}
