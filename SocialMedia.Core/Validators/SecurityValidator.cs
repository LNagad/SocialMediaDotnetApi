using FluentValidation;
using SocialMedia.Core.Aplication.DTOs;

namespace SocialMedia.Core.Validators
{
  public class SecurityValidator : AbstractValidator<SecurityDto>
  {
    public SecurityValidator()
    {
      RuleFor(post => post.User)
        .NotNull()
        .WithMessage("Please ensure you have supplied an User.")
        .Length(5, 20)
        .WithMessage("Please ensure that this field has a minimum length of 5 characters.");

      RuleFor(post => post.UserName)
        .NotNull()
        .WithMessage("Please ensure you have supplied your your name.")
        .Length(5, 20)
        .WithMessage("Please ensure that this field has a minimum length of 5 characters.");

      RuleFor(post => post.Password)
        .NotNull()
        .WithMessage("Please ensure you have supplied the date password.")
        .Length(5, 50)
        .WithMessage("Please ensure that this field has a minimum length of 5 characters.");

    }
  }
}
