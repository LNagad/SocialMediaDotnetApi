using Microsoft.AspNetCore.Identity;
using SocialMedia.Core.Aplication.Enums;
using SocialMedia.Infrastructure.Identity.Entities;

namespace SocialMedia.Infrastructure.Identity.Seeds
{
  public static class DefaultBasicUser
  {
    public static async Task SeedAsync
    (UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      ApplicationUser defaultUser = new()
      {
        UserName = "basicUser",
        Email = "basicuser@email.com",
        FirstName = "John",
        LastName = "Doe",
        EmailConfirmed = true,
        PhoneNumberConfirmed = true,
      };

      if( userManager.Users.All( user => user.Id != defaultUser.Id ) )
      {
        var userExist = await userManager.FindByEmailAsync(defaultUser.Email);
        if(userExist == null )
        {
          await userManager.CreateAsync(defaultUser, "Pa$$w0rd1234");
          await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
        }
        else
        {
          await userManager.AddToRoleAsync(userExist, Roles.Basic.ToString());
        }
      
      }

    }
  }
}
