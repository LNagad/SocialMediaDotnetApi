using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Persistence.Repositories;

namespace SocialMedia.Infrastructure.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly SocialMediaYTContext _context;
    private IPostRepository? _postRepository;
    private IUserRepository? _userRepository;
    private ISecurityRepository? _securityRepository;
    private IGenericRepository<Comment>? _commentRepository;

    public UnitOfWork(SocialMediaYTContext context)
    {
      _context = context;
    }

    public IPostRepository PostRepository => _postRepository ??= new PostRepository(_context);

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

    public ISecurityRepository SecurityRepository => _securityRepository ??= new SecurityRepository(_context);
    public IGenericRepository<Comment> CommentRepository => _commentRepository ??= new GenericRepository<Comment>(_context);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
