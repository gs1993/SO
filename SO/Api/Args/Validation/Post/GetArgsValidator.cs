using Api.Args.Post;
using FluentValidation;

namespace Api.Args.Validation.Post
{
    public sealed class GetArgsValidator : AbstractValidator<GetArgs>
    {
        public GetArgsValidator()
        {
            RuleFor(x => x).NotNull();

            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Limit)
                .GreaterThan(0)
                .LessThanOrEqualTo(10000).WithMessage("Page size must be less than 10000");

            RuleFor(x => x.SearchArgs)
                .Must(x => x == null || x.Length <= 10).WithMessage("Search args must be less than 10");

            RuleForEach(x => x.SearchArgs)
                .Where(x => x != null)
                .ChildRules(searchArgs =>
                {
                    searchArgs.RuleFor(x => x.Field)
                        .NotEmpty()
                        .MaximumLength(150)
                        .WithMessage("bąbeljestsuper");

                    searchArgs.RuleFor(x => x.Value)
                        .NotEmpty()
                        .MaximumLength(250);
                });

            RuleFor(x => x.SortArgs)
                .Must(x => x == null || !string.IsNullOrWhiteSpace(x.Field));

        }
    }
}
