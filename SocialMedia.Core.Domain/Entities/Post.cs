using SocialMedia.Core.Domain.Common;

namespace SocialMedia.Core.Domain.Entities
{
    public partial class Post : BaseEntity
  {
    public Post()
    {
      Comments = new HashSet<Comment>();
    }

    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }

    public virtual User? User { get; set; }
    public virtual ICollection<Comment>? Comments { get; set; }
  }
}
