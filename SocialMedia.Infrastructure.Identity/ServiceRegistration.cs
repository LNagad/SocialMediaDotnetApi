using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SocialMedia.Core.Aplication.DTOs.Account;
using SocialMedia.Core.Domain.Settings;
using SocialMedia.Core.Interfaces.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Identity.Entities;
using SocialMedia.Infrastructure.Identity.Services;


namespace SocialMedia.Infrastructure
{
  //Extension method - decorator
  public static class ServiceRegistration
  {
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration config)
    {
      #region Contexts
      if (config.GetValue<bool>("UseInMemoryDatabase"))
      {
        services.AddDbContext<IdentityContext>(opt => opt.UseInMemoryDatabase("IdentityDb"));
      }
      else
      {
        services.AddDbContext<IdentityContext>(opt =>
        {
          opt.EnableSensitiveDataLogging();
          opt.UseSqlServer(config.GetConnectionString("SocialMediaSomee"),
          m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
        });

      }
      #endregion

      #region Identity & JWT

      services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();

      services.Configure<JWTSettings>(config.GetSection("Authentication"));

      services.AddAuthentication(opt =>
      {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(opt =>
      {
        opt.RequireHttpsMetadata = true; // false in development
        opt.SaveToken = false;
        opt.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuerSigningKey = true,
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true, //si es valido
          ClockSkew = System.TimeSpan.Zero, // si ya expiro, no hay tiempo de gracia
          ValidIssuer = config["Authentication:Issuer"],
          ValidAudience = config["Authentication:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Authentication:SecretKey"]))
        };

        opt.Events = new JwtBearerEvents()
        {
          OnAuthenticationFailed = c =>
          {
            c.NoResult();
            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";
            return c.Response.WriteAsync(c.Exception.ToString());
          },
          OnChallenge = c => // cuando no esta autenticado / token invalido
          {
            c.HandleResponse();
            c.Response.StatusCode = 401;
            c.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new JwtResponse { HasError = true, Error = "You are not Authorized" });
            return c.Response.WriteAsync(result);
          },
          OnForbidden = c => // token valido pero no tiene permisos a la ruta
          {
            c.Response.StatusCode = 403;
            c.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new JwtResponse { HasError = true, Error = "You are not Authorized to access this resource" });
            return c.Response.WriteAsync(result);
          }

        };

      });

      #endregion

      #region Services
      services.AddTransient<IAccountService, AccountService>();
      #endregion
      return services;
    }
  }
}
