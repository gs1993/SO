using Api.Args.User;
using FluentValidation;

namespace Api.Validation.User
{
    public sealed class PermaBanArgsValidator : AbstractValidator<PermaBanArgs>
    {
        public PermaBanArgsValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
