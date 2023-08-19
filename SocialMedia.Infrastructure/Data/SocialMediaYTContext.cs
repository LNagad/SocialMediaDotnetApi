using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Domain.Entities;
using System.Reflection;

namespace SocialMedia.Infrastructure.Data
{
  public partial class SocialMediaYTContext : DbContext
  {
    public SocialMediaYTContext()
    {
    }

    public SocialMediaYTContext(DbContextOptions<SocialMediaYTContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; } = null!;
    public virtual DbSet<Post> Posts { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    public virtual DbSet<Security> Securities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //modelBuilder.ApplyConfiguration(new CommentConfiguration());
      //modelBuilder.ApplyConfiguration(new PostConfiguration());
      //modelBuilder.ApplyConfiguration(new UserConfiguration());

      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

      base.OnModelCreating(modelBuilder);
    }
  }
}
