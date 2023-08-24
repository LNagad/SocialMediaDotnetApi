using SocialMedia.Core.Aplication.DTOs.Account;

namespace SocialMedia.Core.Interfaces.Services
{
  public interface IAccountService
  {
    Task<ConfirmAccountResponse> ConfirmAccountAsync(ConfirmAccountRequest req);
    Task<ForgotPasswordResponese> ForgotPasswordAsync(ForgotPasswordRequest req, string origin);
    Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest req, string origin);
    Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest req);
    Task<AuthenticationResponse> SignInWithEmailAndPasswordAsync(AuthenticationRequest req);
    Task SignOutAsync();
  }
}