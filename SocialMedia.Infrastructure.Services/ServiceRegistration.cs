using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Infrastructure.Services.Interfaces;
using SocialMedia.Infrastructure.Services.Services;

namespace SocialMedia.Infrastructure
{
  public static class ServiceRegistration
  {
    public static void AddServicesLayer(this IServiceCollection services)
    {
      services.AddSingleton<IPasswordService, PasswordService>();
      services.AddSingleton<IUriService>(provider =>
      {
        //get the current HttpContext to build the absolute uri
        var accesor = provider.GetRequiredService<IHttpContextAccessor>();
        var request = accesor.HttpContext.Request;
        var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
        //request.scheme = http | https
        //request.host.ToUriComponent() = localhost:5000 | dns
        return new UriService(absoluteUri);
      });
    }
  }
}
