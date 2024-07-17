using FluentValidation;
using Models.DtoModels;

namespace ToDoAppWebApi.Validations
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(user => user.UserName)
            .NotNull().WithMessage("Username cannot be null.")
            .Matches(@"^(?=[a-zA-Z0-9._]{3,20}$)[a-zA-Z0-9][a-zA-Z0-9._]*$")
            .WithMessage("Username must be between 3 and 20 characters, and can only contain letters, numbers, dots, and underscores.");

            RuleFor(user => user.Password)
                .NotNull().WithMessage("Password cannot be null.")
                .Matches(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,16}$")
                .WithMessage("Password must be between 8 and 16 characters, and contain at least one digit, one lowercase letter, one uppercase letter, one special character, and no spaces.");
        }
    }
}
