using SocialMedia.Core.Domain.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface IPostRepository : IGenericRepository<Post>
  {
    Task<IEnumerable<Post>> GetPostsByUser(int userId);
  }
}
