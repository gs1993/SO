using Api.Args.User;
using FluentValidation;

namespace Api.Args.Validation.User
{
    public sealed class GetLastArgsValidator : AbstractValidator<GetLastArgs>
    {
        public GetLastArgsValidator()
        {
            RuleFor(x => x.Size)
                .GreaterThan(0)
                .LessThanOrEqualTo(1000);
        }
    }
}
