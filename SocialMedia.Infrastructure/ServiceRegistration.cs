using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Infrastructure
{
  public static class ServiceRegistration
  {
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
      services.AddTransient<IPostRepository, PostRepository>();
      services.AddTransient<IUserRepository, UserRepository>();
      services.AddTransient<IUnitOfWork, UnitOfWork>();
      services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

      services.AddDbContext<SocialMediaYTContext>(options =>
      {
        options.UseSqlServer(config.GetConnectionString("LocalDB"));
      });
    }
  }
}
