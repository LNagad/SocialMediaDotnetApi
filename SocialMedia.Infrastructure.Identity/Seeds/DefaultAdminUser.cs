using Microsoft.AspNetCore.Identity;
using SocialMedia.Core.Aplication.Enums;
using SocialMedia.Infrastructure.Identity.Entities;

namespace SocialMedia.Infrastructure.Identity.Seeds
{
  public static class DefaultAdminUser
  {
    public static async Task SeedAsync
    (UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      ApplicationUser defaultUser = new()
      {
        UserName = "adminUser",
        Email = "adminuser@email.com",
        FirstName = "John Admin",
        LastName = "Doe",
        EmailConfirmed = true,
        PhoneNumberConfirmed = true,
      };

      if (userManager.Users.All(user => user.Id != defaultUser.Id))
      {
        var userExist = await userManager.FindByEmailAsync(defaultUser.Email);
        if (userExist == null)
        {
          await userManager.CreateAsync(defaultUser, "Pa$$w0rd1234");

          await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
          await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
          await userManager.AddToRoleAsync(defaultUser, Roles.Moderator.ToString());
        }
      }
    }
  }
}
