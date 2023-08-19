using SocialMedia.Core.Domain.Entities;

namespace SocialMedia.Core.Interfaces
{
  public interface ISecurityRepository : IGenericRepository<Security>
  {
    Task<Security> GetUser(UserLogin login);
  }
}