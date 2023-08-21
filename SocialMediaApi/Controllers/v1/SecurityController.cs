using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Aplication.DTOs;
using SocialMedia.Core.Aplication.Interfaces;
using SocialMedia.Core.Domain.Entities;
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
    private readonly IValidator<SecurityDto> _validator;
    private readonly IPasswordService _passwordService;

    public SecurityController(ISecurityService securityService, IMapper mapper,
      IValidator<SecurityDto> validator, IPasswordService passwordService)
    {
      _securityService = securityService;
      _mapper = mapper;
      _validator = validator;
      _passwordService = passwordService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(SecurityDto securityDto)
    {
      var result = await _validator.ValidateAsync(securityDto);

      if (!result.IsValid)
      {
        return BadRequest(Results.ValidationProblem(result.ToDictionary()));
      }

      var securityEntity = _mapper.Map<Security>(securityDto);
      securityEntity.Password = _passwordService.Hash(securityDto.Password);

      await _securityService.RegisterUser(securityEntity);

      securityDto = _mapper.Map<SecurityDto>(securityEntity);

      var apiResponse = new ApiResponse<SecurityDto>(securityDto);
      return Ok(apiResponse);
    }
  }
}
