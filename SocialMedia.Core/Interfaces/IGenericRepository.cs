using SocialMedia.Core.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface IGenericRepository<Entity> where Entity : BaseEntity
  {
    Task<IEnumerable<Entity>> GetAllAsync();
    Task<Entity> GetByIdAsync(int id);
    Task AddAsync(Entity entity);
    Task<bool> UpdateAsync(Entity entity);
    Task<bool> DeleteAsync(int id);
  }
}
