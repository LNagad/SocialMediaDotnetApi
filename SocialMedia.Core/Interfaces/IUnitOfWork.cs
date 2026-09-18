using SocialMedia.Core.Domain.Entities;
namespace SocialMedia.Core.Interfaces
{
  public interface IUnitOfWork
  {
    IPostRepository PostRepository { get; }
    IUserRepository UserRepository { get; }
    ISecurityRepository SecurityRepository { get; }
    IGenericRepository<Comment> CommentRepository { get; }
    Task SaveChangesAsync();
  }
}
