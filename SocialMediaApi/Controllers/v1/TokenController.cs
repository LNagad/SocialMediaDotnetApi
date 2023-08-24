using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Aplication.DTOs.Account;
using SocialMedia.Core.Aplication.Interfaces.Services;
using SocialMedia.Infrastructure.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMediaApi.Controllers.v1
{

  [ApiVersion("1.0")]

  public class TokenController : BaseApiController
  {
    private readonly IConfiguration _configuration;
    private readonly ISecurityService _securityService;
    private readonly IPasswordService _passwordService;

    public TokenController(IConfiguration configuration,
      ISecurityService securityService, IPasswordService passwordService)
    {
      _configuration = configuration;
      _securityService = securityService;
      _passwordService = passwordService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Authentication(AuthenticationRequest login)
    {
      //todo: if It is a valid user
      var validation = await IsValidUser(login);

      if (validation.Item2.HasError)
      {
        return BadRequest(Results.BadRequest(validation.Item2.Error));
      }

      var token = GenerateToken(validation.Item2);

      return Ok(new { token });
    }


    private async Task<(bool, AuthenticationResponse)> IsValidUser(AuthenticationRequest login)
    {
      var user = await _securityService.SignInWithEmailAndPasswordAsync(login);

      if (user.HasError) return (false, user);
      
      return (true, user);
    }

    private string GenerateToken(AuthenticationResponse security)
    {
      //Header
      var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
      var signingCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);
      var header = new JwtHeader(signingCredentials);

      //Claims
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, security.UserName),
        new Claim(ClaimTypes.Email, security.Email),
      };

      foreach (var role in security.Roles)
      {
        claims.Add(new Claim(ClaimTypes.Role, role));
      }

      //Payload
      var payload = new JwtPayload(
        issuer: _configuration["Authentication:Issuer"],
        audience: _configuration["Authentication:Audience"],
        claims: claims,
        notBefore: DateTime.UtcNow,
        expires: DateTime.UtcNow.AddMinutes(30)
      );

      var token = new JwtSecurityToken(header, payload);

      //searialize the token to a compact jwt format and converts it to a string
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
