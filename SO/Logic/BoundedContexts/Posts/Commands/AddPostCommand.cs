using CSharpFunctionalExtensions;
using Logic.BoundedContexts.Posts.Dtos;
using Logic.BoundedContexts.Posts.Entities;
using Logic.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Commands
{
    public record AddPostCommand : IRequest<Result>
    {
        public string Title { get; init; }
        public string Body { get; init; }
        public DateTime CreateDate { get; init; }
        public int AuthorId { get; init; }
        public string? Tags { get; init; }
        public int? ParentId { get; init; }
    }

    public class AddPostCommandHandler : IRequestHandler<AddPostCommand, Result>
    {
        public readonly DatabaseContext _databaseContext;

        public AddPostCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Result> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            var author = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == request.AuthorId, cancellationToken: cancellationToken);
            if (author == null)
                return Errors.User.NotExists(request.AuthorId);

            var createPostResult = Post.Create(request.Title, request.Body, request.CreateDate, request.AuthorId, "", request.Tags, null);
            if (createPostResult.IsFailure)
                return createPostResult.Error;

            _databaseContext.Attach(createPostResult.Value);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
