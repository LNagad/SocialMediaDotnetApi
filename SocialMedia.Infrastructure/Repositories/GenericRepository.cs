using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Aplication.Exceptions;
using SocialMedia.Core.Domain.Common;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : BaseEntity
  {
    
    private readonly SocialMediaYTContext _dbContext;
    protected DbSet<Entity> _entities; // permite acceder a las entidades de la base de datos desde los hijos de esta clase

    public GenericRepository(SocialMediaYTContext dbContext)
    {
      _dbContext = dbContext;
      _entities = _dbContext.Set<Entity>();
    }

    public virtual IQueryable<Entity> GetAll()
    {
      return _entities.AsNoTracking();
    }

    public virtual async Task<List<Entity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
      return await _entities.AsNoTracking().ToListAsync(cancellationToken); //No Deferred execution
    }

    public virtual async Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties, CancellationToken cancellationToken = default)
    {
      var query = _entities.AsQueryable();

      foreach (var property in properties)
      {
        query = query.Include(property);
      }

      var result = await query.ToListAsync(cancellationToken);

      return result;
    }

    public virtual async Task<Entity> GetByIdWithIncludeAsync(int id, List<string> properties, CancellationToken cancellationToken = default)
    {
      var query = _entities.AsQueryable();

      foreach (var property in properties)
      {
        query = query.Include(property);
      }

      var result = await query.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

      return result;
    }

    public virtual async Task<Entity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
      return await _entities.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task AddAsync(Entity entity, CancellationToken cancellationToken = default)
    {
      await _entities.AddAsync(entity, cancellationToken);
    }

    public virtual Entity Update(Entity entity)
    {
      _entities.Update(entity);

      return entity;
    }

    public virtual void Delete(Entity entity)
    {
      _dbContext.Set<Entity>().Remove(entity);
    }

    public virtual async Task<Entity> FindAndUpdateAsync(Entity entity, int id, CancellationToken cancellationToken = default)
    {
      var entry = await GetByIdAsync(id, cancellationToken);

      if (entry == null) throw new ApiException("Something went wrong when updating", 500);

      _dbContext.Entry(entry).CurrentValues.SetValues(entity);

      return entry;
    }

    public virtual async Task FindAndDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
      var entity = await GetByIdAsync(id, cancellationToken);

      if (entity == null) throw new ApiException("Something went wrong when deleting", 500);
      _entities.Remove(entity);
    }

  }
}
