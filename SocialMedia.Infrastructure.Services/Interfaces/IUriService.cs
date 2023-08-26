using SocialMedia.Core.Aplication.QueryFilters;

namespace SocialMedia.Infrastructure.Services.Interfaces
{
  public interface IUriService
  {
    Uri GetPostPaginationNextUrl(PostQueryFilter filter, string actionUrl, bool nextPage);
    Uri GetPostPaginationPreviousUrl(PostQueryFilter filter, string actionUrl, bool previousPage);
  }
}