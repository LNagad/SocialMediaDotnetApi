using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Domain.Common;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : BaseEntity
  {
    
    private readonly SocialMediaYTContext _context;
    protected DbSet<Entity> _entities;

    public GenericRepository(SocialMediaYTContext context)
    {
      _context = context;
      _entities = _context.Set<Entity>();
    }

    public virtual IEnumerable<Entity> GetAll()
    {
      return _entities.AsNoTracking().AsEnumerable();
    }

    public virtual async Task<Entity> GetByIdAsync(int id)
    {
      return await _entities.FindAsync(id);
    }

    public virtual async Task AddAsync(Entity entity)
    {
      await _entities.AddAsync(entity);
    }

    public virtual void Update(Entity entity)
    {
      _entities.Update(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
      var entity = await GetByIdAsync(id);

      if (entity == null) throw new BusinessException("Something went wrong");
      _entities.Remove(entity);
    }

  }
}
