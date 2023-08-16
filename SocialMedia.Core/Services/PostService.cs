using SocialMedia.Core.Aplication.QueryFilters;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using System.Collections;

namespace SocialMedia.Core.Services
{
  public class PostService : IPostService
  {
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public IEnumerable<Post> GetPosts(PostQueryFilter filters)
    {
      //var watch = System.Diagnostics.Stopwatch.StartNew();
      var posts = _unitOfWork.PostRepository.GetAll();

      if (filters.UserId != null)
      {
        posts = posts.Where(x => x.UserId == filters.UserId);
      }

      if (filters.Date != null)
      {
        //datetime usa horay minutos, por lo que si se quiere filtrar por fecha, se debe usar ToShortDateString()
        posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
      }

      if (filters.Description != null)
      {
        posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
      }

      //watch.Stop();
      //var elapsedMs = watch.ElapsedMilliseconds;

      //Console.WriteLine($"Time elapsed: {elapsedMs} ms");

      return posts;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync(PostQueryFilter filters)
    {
      //var watch = System.Diagnostics.Stopwatch.StartNew();

      var posts = _unitOfWork.PostRepository.GetAllAsync();

      var filteredPosts = new List<Post>();

      await foreach (var post in posts)
      {
        if (filters.UserId != null && post.UserId != filters.UserId)
        {
          continue;
        }

        if (filters.Date != null && post.Date.Date != filters.Date.Value.Date)
        {
          continue;
        }

        if (filters.Description != null && !post.Description.ToLower().Contains(filters.Description.ToLower()))
        {
          continue;
        }

        filteredPosts.Add(post);
      }

      //watch.Stop();
      //var elapsedMs = watch.ElapsedMilliseconds;

      //Console.WriteLine($"Time elapsed: {elapsedMs} ms");

      return filteredPosts;
    }

    public async Task<Post> GetPost(int id)
    {
      return await _unitOfWork.PostRepository.GetByIdAsync(id);
    }

    public async Task InsertPost(Post post)
    {
      var user = await _unitOfWork.UserRepository.GetByIdAsync(post.UserId);

      if (user == null)
      {
        throw new BusinessException("User doesn't exist");
      }

      if (post.Description.Contains("Sexo"))
      {
        throw new BusinessException("Content not allowed");
      }

      var userPosts = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);

      if (userPosts.Count() < 10)
      {
        var lastPost = userPosts.OrderByDescending(x => x.Date).FirstOrDefault();

        // han transcurrido menos de 7 días desde la fecha de la última publicación ?
        if ((DateTime.Now - lastPost.Date).TotalDays < 7)
        {
          throw new BusinessException("You are not able to publish the post");
        }
      }

      await _unitOfWork.PostRepository.AddAsync(post);
      await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> UpdatePost(Post entity)
    {
      _unitOfWork.PostRepository.Update(entity);

      await _unitOfWork.SaveChangesAsync();

      return true;
    }

    public async Task<bool> DeletePost(int id)
    {
      await _unitOfWork.PostRepository.DeleteAsync(id);

      await _unitOfWork.SaveChangesAsync();

      return true;
    }

  
  }
}
