using Api.Args.Post;
using FluentValidation;

namespace Api.Validation.Post
{
    public class CloseArgsValidator : AbstractValidator<CloseArgs>
    {
        public CloseArgsValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
