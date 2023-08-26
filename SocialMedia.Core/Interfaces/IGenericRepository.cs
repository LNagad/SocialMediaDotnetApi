
using SocialMedia.Core.Domain.Common;

namespace SocialMedia.Core.Interfaces
{
  public interface IGenericRepository<Entity> where Entity : BaseEntity
  {
    IEnumerable<Entity> GetAll();
    Task<List<Entity>> GetAllAsync();
    Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties);
    Task<Entity> GetByIdAsync(int id);
    Task<Entity> GetByIdWithIncludeAsync(int id, List<string> properties);
    Task AddAsync(Entity entity);
    Entity Update(Entity entity);
    void Delete(Entity entity);
    Task<Entity> FindAndUpdateAsync(Entity entity, int id);
    Task FindAndDeleteAsync(int id);

  }
}
