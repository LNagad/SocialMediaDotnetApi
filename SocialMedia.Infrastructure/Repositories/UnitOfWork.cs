using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Persistence.Repositories;

namespace SocialMedia.Infrastructure.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly SocialMediaYTContext _context;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISecurityRepository _securityRepository;
    private readonly IGenericRepository<Comment> _commentRepository;

    public UnitOfWork(SocialMediaYTContext context)
    {
      _context = context;
    }

    public IPostRepository PostRepository => _postRepository ?? new PostRepository(_context);

    public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

    public ISecurityRepository SecurityRepository => _securityRepository ?? new SecurityRepository(_context);
    public IGenericRepository<Comment> CommentRepository => new GenericRepository<Comment>(_context);

    public async void Dispose()
    {
      if (_context != null)
      {
        await _context.DisposeAsync();
      } 
    }

    public void SaveChanges()
    {
      _context.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}
