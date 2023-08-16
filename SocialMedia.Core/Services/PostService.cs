using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;

namespace SocialMedia.Core.Services
{
  public class PostService : IPostService
  {
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public IAsyncEnumerable<Post> GetPosts()
    {
      return _unitOfWork.PostRepository.GetAll();
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
