using AuthService.Data.DTOs;
using FluentValidation;

namespace AuthService.Validators
{
    public class SignUpValidator : AbstractValidator<SignUpDTO>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required")
                .Matches(RegexPatterns.Username)
                .WithMessage("Username must be between 8 and 24 characters long and can only contain letters and numbers");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .Matches(RegexPatterns.Password)
                .WithMessage("Password must be between 8 and 24 characters long and can only contain letters, numbers, and the symbols !@#~*.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .Matches(RegexPatterns.Email)
                .WithMessage("Email doesn't match the pattern");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage("Surname is required");
        }
    }
}
