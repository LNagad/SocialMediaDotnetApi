using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Aplication.Interfaces;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Infrastructure.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMediaApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TokenController : ControllerBase
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
    public async Task<IActionResult> Authentication(UserLogin login)
    {
      //todo: if It is a valid user
      var validation = await IsValidUser(login);

      if (validation.Item1)
      {
        var token = GenerateToken(validation.Item2);

        return Ok(new { token });
      }

      return Unauthorized();
    }


    private async Task<(bool, Security)> IsValidUser(UserLogin login)
    {
      var user = await _securityService.GetUser(login);
      if (user == null) return (false, null);
      var isValid = _passwordService.Check(user.Password, login.Password);

      return (isValid, user);
    }

    private string GenerateToken(Security security)
    {
      //Header
      var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
      var signingCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);
      var header = new JwtHeader(signingCredentials);

      //Claims
      var claims = new[]
      {
        new Claim(ClaimTypes.Name, security.UserName),
        new Claim("User", security.User),
        new Claim(ClaimTypes.Role, security.Role.ToString()),
      };

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
