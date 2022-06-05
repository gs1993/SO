using Api.Args.Post;
using FluentValidation;

namespace Api.Validation.User
{
    public sealed class GetArgsValidator : AbstractValidator<GetArgs>
    {
        public GetArgsValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
