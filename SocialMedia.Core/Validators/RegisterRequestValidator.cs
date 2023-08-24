using FluentValidation;
using SocialMedia.Core.Aplication.DTOs.Account;

namespace SocialMedia.Core.Validators
{
  public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
  {
    public RegisterRequestValidator()
    {
      RuleFor(post => post.FirstName)
       .NotNull()
       .WithMessage("Please ensure you have supplied your first name.")
       .Length(5, 20)
       .WithMessage("Please ensure that this field has a minimum length of 5 characters.");

      RuleFor(post => post.LastName)
       .NotNull()
       .WithMessage("Please ensure you have supplied your last name.")
       .Length(5, 20)
       .WithMessage("Please ensure that this field has a minimum length of 5 characters.");

      RuleFor(post => post.UserName)
        .NotNull()
        .WithMessage("Please ensure you have supplied an user name.")
        .Length(5, 20)
        .WithMessage("Please ensure that this field has a minimum length of 5 characters.");

      RuleFor(post => post.PhoneNumber)
      .NotNull()
      .WithMessage("Please ensure you have supplied the phone number.")
      .Matches(@"^(809|829|849)")
      .WithMessage("The phone number must start with 809, 829, or 849")
      .Matches(@"^(?!.*\s)\d{10}$")
      .WithMessage("The phone number must have exactly 10 digits and no spaces.");

      RuleFor(post => post.Email)
        .NotNull()
        .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
        .WithMessage("Invalid email address format.");

      RuleFor(post => post.Password)
        .NotNull()
        .WithMessage("Please ensure you have supplied a password.")
        .Length(8, 50)
        .WithMessage("Please ensure that this field has a minimum length of 8 characters.")
        .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
        .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
        .Matches(@"\d").WithMessage("Password must contain at least one digit.")
        .Matches(@"[@$!%*?&]").WithMessage("Password must contain at least one special character.");
    }
  }
}
