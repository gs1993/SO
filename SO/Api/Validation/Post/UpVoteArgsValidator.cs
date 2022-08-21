﻿using Api.Args.Post;
using FluentValidation;

namespace Api.Validation.Post
{
    public sealed class UpVoteArgsValidator : AbstractValidator<UpVoteArgs>
    {
        public UpVoteArgsValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
