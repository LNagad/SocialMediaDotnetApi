using SocialMedia.Core.Aplication.DTOs.Account;
using SocialMedia.Core.Aplication.Exceptions;
using SocialMedia.Core.Aplication.Interfaces.Services;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Interfaces.Services;

namespace SocialMedia.Core.Aplication.Services
{
  public class SecurityService : ISecurityService
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountService _accountService;
    public SecurityService(IUnitOfWork unitOfWork, IAccountService accountService)
    {
      _unitOfWork = unitOfWork;
      _accountService = accountService;
    }

    public async Task<AuthenticationResponse> SignInWithEmailAndPasswordAsync(AuthenticationRequest login)
    {
      var signInResult = await _accountService.SignInWithEmailAndPasswordAsync(login);

      if (signInResult.HasError)
      {
        throw new ApiException(signInResult.Error);
      }

      return signInResult;
    }

    public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest registerRequest, string origin)
    {
      var registerResponse = await _accountService.RegisterBasicUserAsync(registerRequest, origin);

      if (registerResponse.HasError)
      {
        throw new ApiException(registerResponse.Error);
      }

      return registerResponse;
    }

    public async Task<ConfirmAccountResponse> ConfirmEmailAsync(ConfirmAccountRequest req)
    {
      var result = await _accountService.ConfirmAccountAsync(req);

      if (result.HasError)
      {
        throw new ApiException(result.Error);
      }

      return result;
    }

    public async Task<ForgotPasswordResponese> ForgotPasswordAsync(ForgotPasswordRequest req, string origin)
    {
      var result = await _accountService.ForgotPasswordAsync(req, origin);

      if (result.HasError)
      {
        throw new ApiException(result.Error);
      }

      return result;
    }

    public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest req)
    {
      var result = await _accountService.ResetPasswordAsync(req);

      if (result.HasError)
      {
        throw new ApiException(result.Error);
      }

      return result;
    }

  }
}
