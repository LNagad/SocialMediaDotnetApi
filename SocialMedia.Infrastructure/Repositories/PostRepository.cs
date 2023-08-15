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
      var posts = await _context.Posts.AsNoTracking().ToListAsync();
      //var posts2 = await _context.Set<Post>().AsNoTracking().ToListAsync();

      return posts;
    }

    public async Task<Post> GetPost(int id)
    {
      var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == id);
      return post;
    }

    public async Task InsertPost(Post post)
    {
      await _context.Posts.AddAsync(post);
      await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdatePost(Post post)
    {
      var currenPost = await GetPost(post.PostId);

      if (currenPost == null)
      {
        return false;
      }

      currenPost.Date = post.Date;
      currenPost.Description = post.Description;
      currenPost.Image = post.Image;

      int rows = await _context.SaveChangesAsync();      
      return rows > 0;
    }

    public async Task<bool> DeletePost(int id)
    {
      var currenPost = await GetPost(id);

      if (currenPost == null)
      {
        return false;
      }

      _context.Posts.Remove(currenPost);

      int rows = await _context.SaveChangesAsync();
      return rows > 0;
    }

  }
}
