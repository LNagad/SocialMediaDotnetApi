using SocialMedia.Core.Domain.Entities;

namespace SocialMedia.Core.Aplication.Interfaces
{
  public interface ISecurityService
  {
    Task<Security> GetUser(UserLogin login);
    Task RegisterUser(Security security);
  }
}