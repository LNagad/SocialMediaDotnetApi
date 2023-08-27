using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace SocialMediaApi.Extensions
{
  public static class ServiceExtension
  {
    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
    {

      services.AddSwaggerGen(options =>
      {
        List<string> xmlFiles = Directory
        .GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
        xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));

        options.SwaggerDoc("v1", new OpenApiInfo
        { Title = "Social Media API", 
          Version = "v1", 
          Description = "This api will be responsible for overall data distribution",
          Contact = new OpenApiContact
          {
            Name = "Maycol",
            Email = "maycoldpc@gmail.com",
            Url = new Uri("https://maycol-dev.vercel.app")
          }
        });

        options.EnableAnnotations();
        options.DescribeAllParametersInCamelCase();
        // to add jwt bearer token in swagger
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer",
          BearerFormat = "JWT",
          Description = "Input your bearer token in this format ~ Bearer {your token here}"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
              },
              Scheme = "Bearer",
              Name = "Bearer",
              In = ParameterLocation.Header
            },
            new List<string>()
          }
        });
      });

      return services;
    }

    public static IServiceCollection AddApiVersioningExtension(this IServiceCollection services)
    {
      services.AddApiVersioning(options =>
      {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
      });
     
      return services;
    }
  }
}
