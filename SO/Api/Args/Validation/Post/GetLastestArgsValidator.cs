using Api.Args.Post;
using FluentValidation;

namespace Api.Validation.Post;

public sealed class GetLastestArgsValidator : AbstractValidator<GetLastestArgs>
{
    public GetLastestArgsValidator()
    {
        RuleFor(x => x).NotNull();

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000).WithMessage("Size must be less than 1000");
    }
}
