using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Aplication.Interfaces.Services;
using SocialMedia.Core.Aplication.Services;
using SocialMedia.Core.Services;
using SocialMedia.Core.Validators;
using System.Reflection;
using System.Text;

namespace SocialMedia.Core
{
    public static class ServiceRegistration
  {
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {

      services.AddTransient<IPostService, PostService>();
      services.AddTransient<ISecurityService, SecurityService>();

      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddValidatorsFromAssemblyContaining<PostValidator>();

      return services;
    }

    public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, IConfiguration config)
    {
      services.AddAuthentication(opt =>
      {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(opt =>
      {
        opt.TokenValidationParameters = new()
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = config["Authentication:Issuer"],
          ValidAudience = config["Authentication:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Authentication:SecretKey"])
            )
        };
      });

      return services;
    }
  }
}

