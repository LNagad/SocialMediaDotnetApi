namespace SocialMedia.Infrastructure.Services.Interfaces
{
  public interface IPasswordService
  {
    string Hash(string password);
    bool Check(string hash, string password);
  }
}
