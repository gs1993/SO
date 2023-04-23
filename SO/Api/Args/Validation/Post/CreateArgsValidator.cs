using Api.Args.Post;
using FluentValidation;
using System;

namespace Api.Args.Validation.Post
{
    public class CreateArgsValidator : AbstractValidator<CreateArgs>
    {
        public CreateArgsValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .MustBeEntity(y => Logic.BoundedContexts.Posts.Entities.Post
                    .Create(y.Title, y.Body, DateTime.Now, y.AuthorId, "UserName", y.Tags));
        }
    }
    
}
