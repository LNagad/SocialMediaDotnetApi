using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class UserRepository : GenericRepository<User>, IUserRepository
  {

    public UserRepository(SocialMediaYTContext context) : base(context)
    {

    }

  }
}
