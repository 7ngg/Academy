using DataLayer.Models;
using FluentValidation;
using UserService.Data;

namespace UserService.Validators
{
    class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username cannot be empty")
                .Length(8, 24)
                .WithMessage("Username has to be 4-16 characters long");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty")
                .Length(8, 24)
                .WithMessage("Password has to be 8-24 characters long");

            RuleFor(x => x.Email)
                .EmailAddress();

            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("Role cannot be empty")
                .Must(role =>
                {
                    var min = Enum.GetValues(typeof(Roles)).Cast<Roles>().Min();
                    var max = Enum.GetValues(typeof(Roles)).Cast<Roles>().Max();

                    return role >= min && role <= max;
                })
                .WithMessage("Invalid role");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name cannot be empty");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name cannot be empty");
        }
    }
}
