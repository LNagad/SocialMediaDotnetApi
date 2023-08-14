using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class PostRepository : IPostRepository
  {
    private readonly SocialMediaYTContext _context;

    public PostRepository(SocialMediaYTContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Post>> GetPosts()
    {
      var posts = await _context.Posts.ToListAsync();

      return posts;
    }

    public async Task<Post> GetPost(int id)
    {
      var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);
      return post;
    }

    public async Task InsertPost(Post post)
    {
      _context.Posts.Add(post);
      await _context.SaveChangesAsync();
    }

  }
}
