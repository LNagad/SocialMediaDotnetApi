using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Validators;

namespace SocialMedia.Infrastructure
{
  public static class ServiceRegistration
  {
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      services.AddTransient<IPostRepository, PostRepository>();
      services.AddTransient<IUserRepository, UserRepository>();

      services.AddDbContext<SocialMediaYTContext>(options =>
      {
        options.UseSqlServer(config.GetConnectionString("LocalDB"));
      });

      services.AddValidatorsFromAssemblyContaining<PostValidator>();
    }
  }
}
