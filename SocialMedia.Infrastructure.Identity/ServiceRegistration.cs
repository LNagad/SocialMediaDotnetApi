using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SocialMedia.Core.Aplication.Wrappers;
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
    public static IServiceCollection AddIdentityInfrastructureForApi(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
      #region Contexts

      AddContextConfiguration(services, config);

      #endregion

      #region Identity & JWT

      services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();

      var jwtSettings = config.GetSection("Authentication").Get<JWTSettings>()
        ?? throw new InvalidOperationException(
          "Falta la sección 'Authentication' en la configuración. " +
          "En desarrollo configúrala con 'dotnet user-secrets' y en producción con variables de entorno.");

      if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
      {
        throw new InvalidOperationException(
          "Falta la configuración 'Authentication:SecretKey'. " +
          "En desarrollo: dotnet user-secrets set \"Authentication:SecretKey\" \"<clave>\" --project SocialMediaApi. " +
          "En producción: define la variable de entorno Authentication__SecretKey.");
      }

      if (jwtSettings.SecretKey.Length < 32)
      {
        throw new InvalidOperationException(
          "'Authentication:SecretKey' debe tener al menos 32 caracteres para HMAC-SHA256.");
      }

      if (string.IsNullOrWhiteSpace(jwtSettings.Issuer))
      {
        throw new InvalidOperationException("Falta la configuración 'Authentication:Issuer'.");
      }

      if (string.IsNullOrWhiteSpace(jwtSettings.Audience))
      {
        throw new InvalidOperationException("Falta la configuración 'Authentication:Audience'.");
      }

      services.Configure<JWTSettings>(config.GetSection("Authentication"));

      services.AddAuthentication(opt =>
      {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(opt =>
      {
        opt.RequireHttpsMetadata = !env.IsDevelopment();
        opt.SaveToken = false;
        opt.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuerSigningKey = true,
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true, //si es valido
          ClockSkew = System.TimeSpan.Zero, // si ya expiro, no hay tiempo de gracia
          ValidIssuer = jwtSettings.Issuer,
          ValidAudience = jwtSettings.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
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
            var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
            return c.Response.WriteAsync(result);
          },
          OnForbidden = c => // token valido pero no tiene permisos a la ruta
          {
            c.Response.StatusCode = 403;
            c.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized to access this resource"));
            return c.Response.WriteAsync(result);
          }
        };
      });

      #endregion

      #region Services

      AddServicesConfiguration(services);

      #endregion

      return services;
    }

    #region Private methods

    private static void AddContextConfiguration(this IServiceCollection services, IConfiguration config)
    {
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
    }

    private static void AddServicesConfiguration(this IServiceCollection services)
    {
      services.AddTransient<IAccountService, AccountService>();
    }

    #endregion
  }
}
