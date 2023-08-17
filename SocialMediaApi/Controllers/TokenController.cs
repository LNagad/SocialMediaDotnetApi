using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Domain.Entities;
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
    public TokenController(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    [HttpPost]
    public IActionResult Authentication(UserLogin login)
    {
      //todo: if It is a valid user
      if (IsValidUser(login))
      {
        var token = GenerateToken();

        return Ok(new { token });
      }

      return Unauthorized();
    }


    private bool IsValidUser(UserLogin login)
    {
      return true;
    }

    private string GenerateToken()
    {
      //Header
      var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
      var signingCredentials = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);
      var header = new JwtHeader(signingCredentials);

      //Claims
      var claims = new[]
      {
        new Claim(ClaimTypes.Name, "jose"),
        new Claim(ClaimTypes.Email, ""),
        new Claim(ClaimTypes.Role, "Admin"),
      };

      //Payload
      var payload = new JwtPayload(
        issuer: _configuration["Authentication:Issuer"],
        audience: _configuration["Authentication:Audience"],
        claims: claims,
        notBefore: DateTime.UtcNow,
        expires: DateTime.UtcNow.AddMinutes(10)
      );

      var token = new JwtSecurityToken(header, payload);

      //searialize the token to a compact jwt format and converts it to a string
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
