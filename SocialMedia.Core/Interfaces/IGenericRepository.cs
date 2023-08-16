
using SocialMedia.Core.Domain.Common;

namespace SocialMedia.Core.Interfaces
{
  public interface IGenericRepository<Entity> where Entity : BaseEntity
  {
    IEnumerable<Entity> GetAll();
    IAsyncEnumerable<Entity> GetAllAsync();
    Task<Entity> GetByIdAsync(int id);
    Task AddAsync(Entity entity);
    void Update(Entity entity);
    Task DeleteAsync(int id);
  }
}
