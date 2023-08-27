using Swashbuckle.AspNetCore.SwaggerUI;

namespace SocialMediaApi.Extensions
{
  public static class AppExtensions
  {
    public static IApplicationBuilder UseSwaggerExtension(this IApplicationBuilder app)
    {
      app.UseSwagger();
      app.UseSwaggerUI(options =>
      {
        options.SwaggerEndpoint("../swagger/v1/swagger.json", "Social Media API v1");
        options.RoutePrefix = "swagger";
        options.DefaultModelRendering(ModelRendering.Model);
      });

      return app;
    }
  }
}
