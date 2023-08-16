using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IPostRepository PostRepository { get; }
    IUserRepository UserRepository { get; }
    IGenericRepository<Comment> CommentRepository { get; }
    void SaveChanges();
    Task SaveChangesAsync();
  }
}
