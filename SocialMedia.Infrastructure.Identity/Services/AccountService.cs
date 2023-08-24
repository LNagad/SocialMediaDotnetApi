using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SocialMedia.Core.Aplication.DTOs.Account;
using SocialMedia.Core.Aplication.Enums;
using SocialMedia.Core.Interfaces.Services;
using SocialMedia.Infrastructure.Identity.Entities;
using System.Text;

namespace SocialMedia.Infrastructure.Identity.Services
{
  public class AccountService : IAccountService
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    public async Task<AuthenticationResponse> SignInWithEmailAndPasswordAsync(AuthenticationRequest req)
    {
      var response = new AuthenticationResponse();
      response.HasError = false;

      var user = await _userManager.FindByEmailAsync(req.Email);

      if (user == null)
      {
        response.HasError = true;
        response.Error = $"Not account registered with {req.Email}";
        return response;
      }

      var passwordMatch = await _signInManager.PasswordSignInAsync(user, req.Password, false, false);

      if (!passwordMatch.Succeeded)
      {
        response.HasError = true;
        response.Error = $"Invalid credentials for user {req.Email}";
        return response;
      }

      if (!user.EmailConfirmed)
      {
        response.HasError = true;
        response.Error = $"Account not confirmed for user {req.Email}";
        return response;
      }

      response.Id = user.Id;
      response.Email = user.Email;
      response.UserName = user.UserName;
      //wait until get roles
      var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
      response.Roles = rolesList.ToList();
      response.IsVerified = user.EmailConfirmed;

      return response;
    }

    public async Task SignOutAsync()
    {
      await _signInManager.SignOutAsync();
    }

    public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest req, string origin)
    {
      var response = new RegisterResponse();
      response.HasError = false;

      var emailAlreadyExist = await _userManager.FindByEmailAsync(req.Email);

      if (emailAlreadyExist != null)
      {
        response.HasError = true;
        response.Error = $"User with email {req.Email} already exists";
        return response;
      }

      var userAlreadyExist = await _userManager.FindByNameAsync(req.UserName);

      if (userAlreadyExist != null)
      {
        response.HasError = true;
        response.Error = $"User with username {req.UserName} already exists";
        return response;
      }

      var user = new ApplicationUser
      {
        Email = req.Email,
        FirstName = req.FirstName,
        LastName = req.LastName,
        UserName = req.UserName,
        PhoneNumber = req.PhoneNumber,
        EmailConfirmed = true
      };

      var result = await _userManager.CreateAsync(user, req.Password);

      if (!result.Succeeded)
      {
        response.HasError = true;
        response.Error = $"An error occured trying to register the user.";
        return response;
      }

      await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
      var verificationUri = await SendVerificationEmailUri(user, origin);
      //todo: send email and remove line below
      response.OkResult = verificationUri;

      return response;
    }

    public async Task<ConfirmAccountResponse> ConfirmAccountAsync(ConfirmAccountRequest req)
    {
      var response = new ConfirmAccountResponse();
      response.HasError = false;

      var user = await _userManager.FindByIdAsync(req.UserId);

      if (user == null)
      {
        response.HasError = true;
        response.Error = $"User with id {req.UserId} not found";
        return response;
      }

      var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(req.Token));

      var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

      if (!result.Succeeded)
      {
        response.HasError = true;
        response.Error = $"An error ocurred confirming account for user {user.Email}";
        return response;
      }

      return response;
    }

    public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest req)
    {
      var response = new ResetPasswordResponse();
      response.HasError = false;

      var account = await _userManager.FindByEmailAsync(req.Email);

      if (account == null)
      {
        response.HasError = true;
        response.Error = $"No account registered with email {req.Email}";
        return response;
      }

      var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(req.Token));

      var result = await _userManager.ResetPasswordAsync(account, decodedToken, req.Password);

      if (!result.Succeeded)
      {
        response.HasError = true;
        response.Error = $"An error ocurred resetting password";
        return response;
      }

      return response;
    }

    public async Task<ForgotPasswordResponese> ForgotPasswordAsync(ForgotPasswordRequest req, string origin)
    {
      var response = new ForgotPasswordResponese();
      response.HasError = false;

      var account = await _userManager.FindByEmailAsync(req.Email);

      if (account == null)
      {
        response.HasError = true;
        response.Error = $"No account registered with email {req.Email}";
        return response;
      }

      var forgotPasswordUri = await SendForgotPasswordUri(account, origin);
      //todo: send email and remove line below
      response.OkResult = forgotPasswordUri;

      return response;
    }

    private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
    {
      var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

      var route = "User/ConfirmEmail";

      var Uri = new Uri(string.Concat($"{origin}/", route));

      var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
      verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

      return verificationUri;
    }

    private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
    {
      var code = await _userManager.GeneratePasswordResetTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

      var route = "User/ResetPassword";

      var Uri = new Uri(string.Concat($"{origin}/", route));

      var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

      return verificationUri;
    }
  }
}
