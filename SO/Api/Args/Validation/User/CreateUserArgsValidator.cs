using Api.Args.User;
using FluentValidation;

namespace Api.Args.Validation.User
{
    public sealed class CreateUserArgsValidator : AbstractValidator<CreateUserArgs>
    {
        public CreateUserArgsValidator()
        {
            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .Length(3, 50);
        }
    }
}
