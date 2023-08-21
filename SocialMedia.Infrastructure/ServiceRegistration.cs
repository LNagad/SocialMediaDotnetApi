using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Core.Domain.Settings;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Persistence.Repositories;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Infrastructure
{
  public static class ServiceRegistration
  {
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
    {
      services.Configure<PaginationSettings>(config.GetSection("PaginationOptions"));
      services.Configure<PasswordSettings>(config.GetSection("PasswordOptions"));

      services.AddTransient<IPostRepository, PostRepository>();
      services.AddTransient<IUserRepository, UserRepository>();
      services.AddTransient<ISecurityRepository, SecurityRepository>();
      services.AddTransient<IUnitOfWork, UnitOfWork>();
      services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

      services.AddDbContext<SocialMediaYTContext>(options =>
      {
        options.UseSqlServer(config.GetConnectionString("LocalDB"));
      });

      return services;
    }
  }
}
