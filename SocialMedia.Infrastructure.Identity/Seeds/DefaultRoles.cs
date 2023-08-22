using Microsoft.AspNetCore.Identity;
using SocialMedia.Core.Aplication.Enums;
using SocialMedia.Infrastructure.Identity.Entities;

namespace SocialMedia.Infrastructure.Identity.Seeds
{
  public static class DefaultRoles
  {
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
      await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
      await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
    }
  }
}
