namespace SocialMedia.Core.Domain.Settings
{
  public class PasswordSettings
  {
    public int SaltSize { get; set; }
    public int KeySize { get; set; }
    public int Iterations { get; set; }
  }
}
