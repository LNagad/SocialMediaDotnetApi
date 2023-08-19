using Microsoft.Extensions.Options;
using SocialMedia.Core.Domain.Settings;
using SocialMedia.Infrastructure.Services.Interfaces;
using System.Security.Cryptography;

namespace SocialMedia.Infrastructure.Services.Services
{

  public class PasswordService : IPasswordService
  {
    private readonly PasswordSettings _options;

    public PasswordService(IOptions<PasswordSettings> passwordSettings)
    {
      _options = passwordSettings.Value;
    }

    public bool Check(string hash, string password)
    {
      var parts = hash.Split('.');
      if (parts.Length != 3)
      {
        throw new FormatException("Unexpected hash format.");
      }

      var iterations = Convert.ToInt32(parts[0]);
      var salt = Convert.FromBase64String(parts[1]);
      var key = Convert.FromBase64String(parts[2]); //db password

      using (var algorithm = new Rfc2898DeriveBytes(
             password,
              salt,
              iterations,
              HashAlgorithmName.SHA512
          ))
      {
        var keyToCheck = algorithm.GetBytes(_options.KeySize);

        return keyToCheck.SequenceEqual(key); //true or false
      }
    }

    public string Hash(string password)
    {
      using (var algorithm = new Rfc2898DeriveBytes(
               password,
                _options.SaltSize,
                _options.Iterations,
                HashAlgorithmName.SHA512
            ))
      {
        var key = Convert.ToBase64String(algorithm.GetBytes(_options.KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{_options.Iterations}.{salt}.{key}";
      }
    }
  }
}
