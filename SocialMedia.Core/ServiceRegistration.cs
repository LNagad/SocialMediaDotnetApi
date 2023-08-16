using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Core.Validators;

namespace SocialMedia.Core
{
  public static class ServiceRegistration
  {
    public static void AddServicesLayer(this IServiceCollection services)
    {
   
      services.AddTransient<IPostService, PostService>();

      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddValidatorsFromAssemblyContaining<PostValidator>();

    }
  }
}
