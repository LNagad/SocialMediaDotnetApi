using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;

namespace SocialMedia.Core
{
  public static class ServiceRegistration
  {
    public static void AddServicesLayer(this IServiceCollection services)
    {
   
      services.AddTransient<IPostService, PostService>();
    
    }
  }
}
