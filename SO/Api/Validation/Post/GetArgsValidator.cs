using Api.Args.Post;
using FluentValidation;

namespace Api.Validation.Post
{
    public class GetArgsValidator : AbstractValidator<GetArgs>
    {
        public GetArgsValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
