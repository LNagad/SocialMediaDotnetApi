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
    public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration config)
    {
      services.Configure<PaginationSettings>(config.GetSection("PaginationOptions"));
      services.Configure<PasswordSettings>(config.GetSection("PasswordOptions"));

      services.AddScoped<IPostRepository, PostRepository>();
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<ISecurityRepository, SecurityRepository>();
      services.AddScoped<IUnitOfWork, UnitOfWork>();
      services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

      services.AddDbContext<SocialMediaYTContext>(options =>
      {
        options.UseSqlServer(config.GetConnectionString("SocialMediaSomee"));
      });

      return services;
    }
  }
}
