using SocialMedia.Core.Domain.Entities;
namespace SocialMedia.Core.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    IPostRepository PostRepository { get; }
    IUserRepository UserRepository { get; }
    ISecurityRepository SecurityRepository { get; }
    IGenericRepository<Comment> CommentRepository { get; }
    void SaveChanges();
    Task SaveChangesAsync();
  }
}
