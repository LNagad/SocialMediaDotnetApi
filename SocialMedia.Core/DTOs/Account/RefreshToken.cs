namespace SocialMedia.Core.Aplication.DTOs.Account
{
  public class RefreshToken
  {
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; } // when the token is revoked / when the token got expired
    public string ReplacedByToken { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
  }
}
