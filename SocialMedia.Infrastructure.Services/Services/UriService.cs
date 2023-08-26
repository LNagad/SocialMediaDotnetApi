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

    public Uri GetPostPaginationNextUrl(PostQueryFilter filter, string actionUrl, bool nextPage)
    {
      string pagination = nextPage ? $"?pageSize={filter.PageSize}&pageNumber={filter.PageNumber + 1}" : "";
      string baseUrl = $"{_baseUri}{actionUrl}{pagination}";
      return new Uri(baseUrl);
    }

    public Uri GetPostPaginationPreviousUrl(PostQueryFilter filter, string actionUrl, bool previousPage)
    {
      string pagination = previousPage ? $"?pageSize={filter.PageSize}&pageNumber={filter.PageNumber - 1}" : "";
      string baseUrl = $"{_baseUri}{actionUrl}{pagination}";
      return new Uri(baseUrl);
    }


  }
}
