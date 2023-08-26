namespace SocialMedia.Core.Aplication.Features.Posts.Commands.UpdatePostById
{
  public class PostUpdateResponse
  {
    public int PostId { get; set; }
    public int UserId { get; set; }
    public DateTime? Date { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
  }
}
