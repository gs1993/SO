using Api.Args.Post;
using FluentValidation;

namespace Api.Validation.Post
{
    public sealed class GetListArgsValidator : AbstractValidator<GetListArgs>
    {
        public GetListArgsValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(1000).WithMessage("Page size must be less than 1000");

        }
    }
}
