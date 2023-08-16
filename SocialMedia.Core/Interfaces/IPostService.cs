using SocialMedia.Core.Domain.Entities;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostService
    {
    IAsyncEnumerable<Post> GetPosts();
    Task<Post> GetPost(int id);
    Task InsertPost(Post post);
    Task<bool> UpdatePost(Post post);
    Task<bool> DeletePost(int id);
  }
}