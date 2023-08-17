using SocialMedia.Core.Aplication.QueryFilters;

namespace SocialMedia.Infrastructure.Services.Interfaces
{
    public interface IUriService
    {
        Uri GetPostPaginationUrl(PostQueryFilter filter, string actionUrl);
    }
}