
using SocialMedia.Core.Domain.Common;

namespace SocialMedia.Core.Interfaces
{
  public interface IGenericRepository<Entity> where Entity : BaseEntity
  {
    IQueryable<Entity> GetAll();
    Task<List<Entity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties, CancellationToken cancellationToken = default);
    Task<Entity> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Entity> GetByIdWithIncludeAsync(int id, List<string> properties, CancellationToken cancellationToken = default);
    Task AddAsync(Entity entity, CancellationToken cancellationToken = default);
    Entity Update(Entity entity);
    void Delete(Entity entity);
    Task<Entity> FindAndUpdateAsync(Entity entity, int id, CancellationToken cancellationToken = default);
    Task FindAndDeleteAsync(int id, CancellationToken cancellationToken = default);

  }
}
