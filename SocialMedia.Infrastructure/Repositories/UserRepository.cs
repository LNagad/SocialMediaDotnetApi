using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class UserRepository : IUserRepository
  {

    private readonly SocialMediaYTContext _context;

    public UserRepository(SocialMediaYTContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
      var users = await _context.Users.AsNoTracking().ToListAsync();
      return users;
    }

    public async Task<User> GetUser(int id)
    {
      var user = await _context.Users.FirstOrDefaultAsync(p => p.UserId == id);
      return user;
    }

    //public async Task InsertPost(Post post)
    //{
    //  await _context.Posts.AddAsync(post);
    //  await _context.SaveChangesAsync();
    //}

    //public async Task<bool> UpdatePost(Post post)
    //{
    //  var currenPost = await GetPost(post.PostId);

    //  if (currenPost == null)
    //  {
    //    return false;
    //  }

    //  currenPost.Date = post.Date;
    //  currenPost.Description = post.Description;
    //  currenPost.Image = post.Image;

    //  int rows = await _context.SaveChangesAsync();
    //  return rows > 0;
    //}

    //public async Task<bool> DeletePost(int id)
    //{
    //  var currenPost = await GetPost(id);

    //  if (currenPost == null)
    //  {
    //    return false;
    //  }

    //  _context.Posts.Remove(currenPost);

    //  int rows = await _context.SaveChangesAsync();
    //  return rows > 0;
    //}

  }
}
