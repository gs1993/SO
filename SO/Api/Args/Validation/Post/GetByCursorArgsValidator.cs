using Api.Args.Post;
using FluentValidation;

namespace Api.Args.Validation.Post
{
    public sealed class GetByCursorArgsValidator : AbstractValidator<GetByCursorArgs>
    {
        public GetByCursorArgsValidator()
        {
            RuleFor(x => x).NotNull();

            RuleFor(x => x.Cursor)
                .GreaterThan(0)
                .When(x => x.Cursor != null);

            RuleFor(x => x.Limit)
                .GreaterThan(0)
                .LessThanOrEqualTo(10000).WithMessage("Page size must be less than 10000");

        }
    }
}
