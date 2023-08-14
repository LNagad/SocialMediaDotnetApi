using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface IPostRepository
  {
    Task<IEnumerable<Post>> GetPosts();
    Task<Post> GetPost(int id);
    Task InsertPost(Post post);
  }
}
