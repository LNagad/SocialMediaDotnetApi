using SocialMedia.Core.Domain.Common;
using SocialMedia.Core.Domain.Enums;

namespace SocialMedia.Core.Domain.Entities
{
  public class Security : BaseEntity
  {
    public string User { get; set; }
    public string UserName { get; set; }

    public string Password { get; set; }

    public RoleType Role { get; set; }
  }
}
