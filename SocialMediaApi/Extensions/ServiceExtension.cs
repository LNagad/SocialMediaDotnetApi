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

        options.DescribeAllParametersInCamelCase();
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
