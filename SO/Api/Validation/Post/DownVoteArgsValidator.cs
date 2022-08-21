using Api.Args.Post;
using FluentValidation;

namespace Api.Validation.Post
{
    public sealed class DownVoteArgsValidator : AbstractValidator<DownVoteArgs>
    {
        public DownVoteArgsValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
