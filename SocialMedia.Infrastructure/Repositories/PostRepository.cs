using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class PostRepository : GenericRepository<Post>, IPostRepository
  {
    public PostRepository(SocialMediaYTContext context) : base(context) { }

    public async Task<IEnumerable<Post>> GetPostsByUser(int userId)
    {
      return await _entities.Where(x => x.UserId == userId).ToListAsync();
    }

    // seems to be slower than GetAll
    public async IAsyncEnumerable<Post> GetAllAsyncEnumerable()
    {
      await foreach (var Post in _entities.AsAsyncEnumerable())
      {
        yield return Post;
      }
    }

    public async Task<IAsyncEnumerable<Post>> GetAllAsyncEnumerableTask()
    {
      return _entities.AsAsyncEnumerable();
    }

    public async Task<List<Post>> GetAllAsyncList()
    {
      return await _entities.AsNoTracking().ToListAsync();
    }
  }
}
