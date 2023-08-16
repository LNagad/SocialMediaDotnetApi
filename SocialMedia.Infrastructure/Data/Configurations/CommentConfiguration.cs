﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Domain.Entities;

namespace SocialMedia.Infrastructure.Data.Configurations
{
  public class CommentConfiguration : IEntityTypeConfiguration<Comment>
  {
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
      builder.HasKey(e => e.Id);

      builder.ToTable("Comentario");

      builder.Property(e => e.Id)
          .HasColumnName("IdComentario")
          .ValueGeneratedNever();

      builder.Property(e => e.PostId)
          .HasColumnName("IdPublicacion");

      builder.Property(e => e.UserId)
          .HasColumnName("IdUsuario");

      builder.Property(e => e.Description)
          .HasColumnName("Descripcion")
          .HasMaxLength(500)
          .IsUnicode(false);

      builder.Property(e => e.Date)
          .HasColumnName("Fecha")
          .HasColumnType("datetime");

      builder.Property(e => e.IsActive)
          .HasColumnName("Activo");

      builder.HasOne(d => d.Post)
          .WithMany(p => p.Comments)
          .HasForeignKey(d => d.PostId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_Comentario_Publicacion");

      builder.HasOne(d => d.User)
          .WithMany(p => p.Comments)
          .HasForeignKey(d => d.UserId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("FK_Comentario_Usuario");
    }
  }
}
