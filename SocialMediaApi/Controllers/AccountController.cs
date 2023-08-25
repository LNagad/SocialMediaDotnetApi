using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Aplication.DTOs.Account;
using SocialMedia.Core.Aplication.Interfaces.Services;
using SocialMedia.Infrastructure.Services.Interfaces;
using SocialMediaApi.Responses;

namespace SocialMediaApi.Controllers
{

  [ApiController]
  [Produces("application/json")]
  [Route("api/[controller]")]

  public class AccountController: ControllerBase
  {
    private readonly ISecurityService _securityService;
    private readonly IMapper _mapper;
    private readonly IValidator<RegisterRequest> _validator;
    private readonly IPasswordService _passwordService;

    public AccountController(ISecurityService securityService, IMapper mapper,
      IValidator<RegisterRequest> validator, IPasswordService passwordService)
    {
      _securityService = securityService;
      _mapper = mapper;
      _validator = validator;
      _passwordService = passwordService;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest login)
    {
      var authenticationResponse = await _securityService.SignInWithEmailAndPasswordAsync(login);

      var apiResponse = new ApiResponse<AuthenticationResponse>(authenticationResponse);

      return Ok(apiResponse);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(RegisterRequest registerRequest)
    {
      var result = await _validator.ValidateAsync(registerRequest);

      if (!result.IsValid)
      {
        return BadRequest(Results.ValidationProblem(result.ToDictionary()));
      }

      var origin = Request.Headers["origin"];

      Uri uriResult;
      bool isValidUri = Uri.TryCreate(origin, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

      if (!isValidUri)
      {
        // La cadena no es una URI válida
        return BadRequest("Origin header is not valid");
      }

      var registerResponse = await _securityService.RegisterUserAsync(registerRequest, origin);

      var apiResponse = new ApiResponse<RegisterResponse>(registerResponse);
      return Ok(apiResponse);
    }

    [AllowAnonymous]
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmAccountRequest req)
    {
      var result = await _securityService.ConfirmEmailAsync(req);

      var apiResponse = new ApiResponse<ConfirmAccountResponse>(result);
      return Ok(apiResponse);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest req)
    {
      var origin = Request.Headers["origin"];
      Uri uriResult;
      bool isValidUri = Uri.TryCreate(origin, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

      if (!isValidUri)
      {
        // La cadena no es una URI válida
        return BadRequest(Results.BadRequest("Origin header is not valid"));
      }

      var result = await _securityService.ForgotPasswordAsync(req, origin);

      var apiResponse = new ApiResponse<ForgotPasswordResponese>(result);
      return Ok(apiResponse);
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest req)
    {
      var result = await _securityService.ResetPasswordAsync(req);

      var apiResponse = new ApiResponse<ResetPasswordResponse>(result);
      return Ok(apiResponse);
    }
  }
}
