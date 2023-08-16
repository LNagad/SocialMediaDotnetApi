using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;

namespace SocialMedia.Infrastructure.Repositories
{
  public class PostRepository : GenericRepository<Post>, IPostRepository
  {
    public PostRepository(SocialMediaYTContext context) : base(context)
    {
    }
    
  }
}
