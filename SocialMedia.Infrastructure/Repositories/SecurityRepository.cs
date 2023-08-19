using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Repositories;

namespace SocialMedia.Infrastructure.Persistence.Repositories
{
  public class SecurityRepository : GenericRepository<Security>, ISecurityRepository
  {
    public SecurityRepository(SocialMediaYTContext context) : base(context) { }

    public async Task<Security> GetUser(UserLogin login)
    {
      return await _entities.FirstOrDefaultAsync(x => x.User == login.User);
    }

  }
}
