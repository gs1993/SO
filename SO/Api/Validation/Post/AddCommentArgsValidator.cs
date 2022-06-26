using Api.Args.Post;
using FluentValidation;

namespace Api.Validation.Post
{
    public sealed class AddCommentArgsValidator : AbstractValidator<AddCommentArgs>
    {
        public AddCommentArgsValidator()
        {
            RuleFor(x => x.Comment).NotEmpty();
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
