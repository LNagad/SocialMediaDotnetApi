using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Domain.Entities;
using SocialMedia.Core.Domain.Enums;

namespace SocialMedia.Infrastructure.Persistence.Data.Configurations
{
  public class SecurityConfiguration : IEntityTypeConfiguration<Security>
  {
    public void Configure(EntityTypeBuilder<Security> builder)
    {
      builder.ToTable("Seguridad");

      builder.HasKey(e => e.Id);

      builder.Property(e => e.Id)
      .HasColumnName("IdSeguridad");

      builder.Property(e => e.User)
      .HasColumnName("Usuario")
      .HasMaxLength(20)
      .IsUnicode(false)
      .IsRequired();

      builder.Property(e => e.UserName)
      .HasColumnName("NombreUsuario")
      .HasMaxLength(30)
      .IsUnicode(false)
      .IsRequired();

      builder.Property(e => e.Password)
      .HasColumnName("Clave")
      .HasMaxLength(50)
      .IsUnicode(true);

      builder.Property(e => e.Role)
      .HasColumnName("ROL")
      .HasMaxLength(15)
      .IsUnicode(false)
      .HasConversion( 
        x => x.ToString(), 
        x => (RoleType)Enum.Parse(typeof(RoleType), x) 
      );
    }
  }
}
