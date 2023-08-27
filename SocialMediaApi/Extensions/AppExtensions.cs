using SocialMediaApi.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SocialMediaApi.Extensions
{
  public static class AppExtensions
  {
    public static void UseSwaggerExtension(this IApplicationBuilder app)
    {
      app.UseSwagger();
      app.UseSwaggerUI(options =>
      {
        options.SwaggerEndpoint("../swagger/v1/swagger.json", "Social Media API v1");
        options.RoutePrefix = "swagger";
        options.DefaultModelRendering(ModelRendering.Model);
      });

    }

    public static void UseErrorHandlerMiddleware(this IApplicationBuilder app)
    {
      app.UseMiddleware<ErrorHandleMiddleware>();

    }
  }
}
