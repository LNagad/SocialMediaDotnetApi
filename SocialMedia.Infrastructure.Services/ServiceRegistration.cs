using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Infrastructure.Services.Interfaces;
using SocialMedia.Infrastructure.Services.Services;

namespace SocialMedia.Infrastructure
{
  public static class ServiceRegistration
  {
    public static IServiceCollection AddServicesInfrastructure(this IServiceCollection services)
    {
      services.AddSingleton<IPasswordService, PasswordService>();
      services.AddSingleton<IUriService, UriService>();

      return services;
    }
  }
}
