using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : BaseEntity
  {
    
    private readonly SocialMediaYTContext _context;
    private DbSet<Entity> _entities;

    public GenericRepository(SocialMediaYTContext context)
    {
      _context = context;
      _entities = _context.Set<Entity>();
    }

    public virtual async Task<IEnumerable<Entity>> GetAllAsync()
    {
      return await _entities.AsNoTracking().ToListAsync();
    }

    public virtual async Task<Entity> GetByIdAsync(int id)
    {
      return await _entities.FindAsync(id);
    }

    public virtual async Task AddAsync(Entity entity)
    {
      await _entities.AddAsync(entity);
      await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> UpdateAsync(Entity entity)
    {
      _entities.Update(entity);

      int rows = await _context.SaveChangesAsync();
      return rows > 0;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
      var entity = await GetByIdAsync(id);
      _entities.Remove(entity);

      int rows = await _context.SaveChangesAsync();
      return rows > 0;
    }

  }
}
