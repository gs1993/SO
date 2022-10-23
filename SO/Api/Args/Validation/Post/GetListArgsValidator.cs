using Api.Args.Post;
using FluentValidation;

namespace Api.Args.Validation.Post
{
    public sealed class GetArgsValidator : AbstractValidator<GetArgs>
    {
        public GetArgsValidator()
        {
            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Limit)
                .GreaterThan(0)
                .LessThanOrEqualTo(10000).WithMessage("Page size must be less than 10000");

        }
    }
}
