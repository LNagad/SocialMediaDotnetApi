using SocialMedia.Core.Aplication.QueryFilters;
using SocialMedia.Core.Domain.Entities;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostService
    {
    IEnumerable<Post> GetPosts(PostQueryFilter filters);
    Task<IEnumerable<Post>> GetPostsAsync(PostQueryFilter filters);
    Task<Post> GetPost(int id);
    Task InsertPost(Post post);
    Task<bool> UpdatePost(Post post);
    Task<bool> DeletePost(int id);
  }
}