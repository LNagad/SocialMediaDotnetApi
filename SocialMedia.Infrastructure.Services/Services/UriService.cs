using SocialMedia.Core.Aplication.QueryFilters;
using SocialMedia.Infrastructure.Services.Interfaces;

namespace SocialMedia.Infrastructure.Services.Services
{
    public class UriService : IUriService
  {

    private readonly string _baseUri;

    public UriService(string uri)
    {
      _baseUri = uri;
    }

    public Uri GetPostPaginationUrl(PostQueryFilter filter, string actionUrl)
    {
      string baseUrl = $"{_baseUri}{actionUrl}";
      return new Uri(baseUrl);
    }


  }
}
