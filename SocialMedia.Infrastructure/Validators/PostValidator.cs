using FluentValidation;
using SocialMedia.Core.DTOs;

namespace SocialMedia.Infrastructure.Validators
{
  public class PostValidator: AbstractValidator<PostDto>
  {
    public PostValidator()
    {
      RuleFor(post => post.Description)
        .NotNull()
        .WithMessage("Please ensure you have supplied a description.")
        .Length(10, 500)
        .WithMessage("Please ensure that this field has a minimum length of 10 characters.");

      RuleFor(post => post.Image)
        .NotNull()
        .WithMessage("Please ensure you have supplied an image.");
    }
  }
}
