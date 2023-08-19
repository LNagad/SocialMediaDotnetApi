using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Core.Aplication.Interfaces;
using SocialMedia.Core.Aplication.Services;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Core.Validators;

namespace SocialMedia.Core
{
  public static class ServiceRegistration
  {
    public static void AddApplicationLayer(this IServiceCollection services)
    {
   
      services.AddTransient<IPostService, PostService>();
      services.AddTransient<ISecurityService, SecurityService>();

      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddValidatorsFromAssemblyContaining<PostValidator>();

    }
  }
}
