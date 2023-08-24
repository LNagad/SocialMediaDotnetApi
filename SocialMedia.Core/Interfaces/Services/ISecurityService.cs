using SocialMedia.Core.Aplication.DTOs.Account;

namespace SocialMedia.Core.Aplication.Interfaces.Services
{
  public interface ISecurityService
  {
    Task<AuthenticationResponse> SignInWithEmailAndPasswordAsync(AuthenticationRequest login);
    Task<RegisterResponse> RegisterUserAsync(RegisterRequest registerRequest, string origin);
    Task<ConfirmAccountResponse> ConfirmEmailAsync(ConfirmAccountRequest req);
    Task<ForgotPasswordResponese> ForgotPasswordAsync(ForgotPasswordRequest req, string origin);
    Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest req);
  }
}