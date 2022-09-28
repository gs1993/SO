using Api.Args.Post;
using FluentValidation;

namespace Api.Validation.Post
{
    public sealed class GetArgsValidator : AbstractValidator<GetArgs>
    {
        public GetArgsValidator()
        {
            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Limit)
                .GreaterThan(0)
                .LessThanOrEqualTo(1000).WithMessage("Page size must be less than 1000");

        }
    }
}
