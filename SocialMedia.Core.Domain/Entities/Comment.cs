using SocialMedia.Core.Domain.Common;

namespace SocialMedia.Core.Domain.Entities
{
    public partial class Comment : BaseEntity
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
