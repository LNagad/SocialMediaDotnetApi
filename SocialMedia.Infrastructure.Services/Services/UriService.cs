using Microsoft.AspNetCore.Http;
using SocialMedia.Core.Aplication.QueryFilters;
using SocialMedia.Infrastructure.Services.Interfaces;

namespace SocialMedia.Infrastructure.Services.Services
{
  public class UriService : IUriService
  {

    private readonly IHttpContextAccessor _httpContextAccessor;

    public UriService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    // Se resuelve por llamada, no al construir: el scheme y el host varian entre
    // requests y solo existen dentro de uno.
    private string BaseUri
    {
      get
      {
        var request = _httpContextAccessor.HttpContext!.Request;
        return string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
      }
    }

    public Uri GetPostPaginationNextUrl(PostQueryFilter filter, string actionUrl, bool nextPage)
    {
      string pagination = nextPage ? $"?pageSize={filter.PageSize}&pageNumber={filter.PageNumber + 1}" : "";
      string baseUrl = $"{BaseUri}{actionUrl}{pagination}";
      return new Uri(baseUrl);
    }

    public Uri GetPostPaginationPreviousUrl(PostQueryFilter filter, string actionUrl, bool previousPage)
    {
      string pagination = previousPage ? $"?pageSize={filter.PageSize}&pageNumber={filter.PageNumber - 1}" : "";
      string baseUrl = $"{BaseUri}{actionUrl}{pagination}";
      return new Uri(baseUrl);
    }


  }
}
