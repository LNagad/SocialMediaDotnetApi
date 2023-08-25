﻿namespace SocialMedia.Core.Domain.Settings
{
  public  class JWTSettings
  {
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int DurationInMinutes { get; set; }
  }
}
