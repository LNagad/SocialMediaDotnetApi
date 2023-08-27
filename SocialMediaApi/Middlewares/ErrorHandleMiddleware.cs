using SocialMedia.Core.Aplication.Exceptions;
using SocialMedia.Core.Aplication.Wrappers;
using System.Net;
using System.Text.Json;

namespace SocialMediaApi.Middlewares
{
  public class ErrorHandleMiddleware
  {
    private readonly RequestDelegate _next;

    public ErrorHandleMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception error)
      {
        var response = httpContext.Response;
        response.ContentType = "application/json";

        var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

        switch (error)
        {
          case ApiException e:
            switch (e.ErrorCode)
            {
              case (int)HttpStatusCode.BadRequest:
                response.StatusCode = (int)HttpStatusCode.BadRequest; //400
                break;

              case (int)HttpStatusCode.NotFound:
                response.StatusCode = (int)HttpStatusCode.NotFound; //404
                break;

              default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError; //500
                break;
            }
            break;

          case KeyNotFoundException e:
            response.StatusCode = (int)HttpStatusCode.NotFound; //404
            break;

          default:
            response.StatusCode = (int)HttpStatusCode.InternalServerError; //500
            break;
        }

        var result = JsonSerializer.Serialize(responseModel);
        await response.WriteAsync(result);

      }
    }
  }
}
