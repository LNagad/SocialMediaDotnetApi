using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Aplication.DTOs.Account;
using SocialMedia.Core.Aplication.Interfaces.Services;
using SocialMedia.Core.Domain.Enums;
using SocialMedia.Infrastructure.Services.Interfaces;
using SocialMediaApi.Responses;

namespace SocialMediaApi.Controllers.v1
{

  [ApiVersion("1.0")]
  [Authorize(Roles = nameof(RoleType.Admin))]

  public class SecurityController : BaseApiController
  {
    private readonly ISecurityService _securityService;
    private readonly IMapper _mapper;
    private readonly IValidator<RegisterRequest> _validator;
    private readonly IPasswordService _passwordService;

    public SecurityController(ISecurityService securityService, IMapper mapper,
      IValidator<RegisterRequest> validator, IPasswordService passwordService)
    {
      _securityService = securityService;
      _mapper = mapper;
      _validator = validator;
      _passwordService = passwordService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(RegisterRequest registerRequest)
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

      var registerResponse =  await _securityService.RegisterUserAsync(registerRequest, origin);

      var apiResponse = new ApiResponse<RegisterResponse>(registerResponse);
      return Ok(apiResponse);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmAccountRequest req)
    {
      var result = await _securityService.ConfirmEmailAsync(req);

      var apiResponse = new ApiResponse<ConfirmAccountResponse>(result);
      return Ok(apiResponse);
    }

    [AllowAnonymous]
    [HttpPost("ForgotPassword")]
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
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest req)
    {
      var result = await _securityService.ResetPasswordAsync(req);

      var apiResponse = new ApiResponse<ResetPasswordResponse>(result);
      return Ok(apiResponse);
    }
  }
}
