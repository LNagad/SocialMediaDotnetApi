namespace SocialMedia.Core.Aplication.DTOs.Account
{
  public class AuthenticationResponse
  {
    public string Id { get; set; }              
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public string Password { get; set; }
    public bool IsVerified { get; set; }
    public bool HasError { get; set; }
    public string Error { get; set; }

  }
}
