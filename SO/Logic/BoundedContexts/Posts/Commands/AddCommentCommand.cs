﻿using CSharpFunctionalExtensions;
using Logic.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Commands
{
    public record AddCommentCommand : IRequest<Result>
    {
        public int PostId { get; init; }
        public int UserId { get; init; }
        public string Comment { get; init; }
    }

    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result>
    {
        private readonly DatabaseContext _databaseContext;

        public AddCommentCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        }

        public async Task<Result> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken);
            if (user == null)
                return Result.Failure("User does not exists");

            var post = await _databaseContext.Posts.FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken);
            if (post == null)
                return Result.Failure("Post does not exists");

            _databaseContext.Entry(post).Collection(x => x.Comments).Load();

            var result = post.AddComment(user, request.Comment);
            if (result.IsFailure)
                return result;

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    }
}
