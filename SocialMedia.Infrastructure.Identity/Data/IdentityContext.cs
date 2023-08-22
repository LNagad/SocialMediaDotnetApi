using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Infrastructure.Identity.Entities;

namespace SocialMedia.Infrastructure.Data
{
  public class IdentityContext : IdentityDbContext<ApplicationUser>
  {

    public IdentityContext(DbContextOptions<IdentityContext> options): base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.HasDefaultSchema("Identity");

      modelBuilder.Entity<ApplicationUser>(entity => entity.ToTable(name: "Users"));

      modelBuilder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));

      modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));

      modelBuilder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
    }
  }
}
