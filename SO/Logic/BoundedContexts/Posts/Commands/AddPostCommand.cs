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
    public record CreatePostCommand : IRequest<Result>
    {
        public string Title { get; init; }
        public string Body { get; init; }
        public int AuthorId { get; init; }
        public string? Tags { get; init; }
        public int? ParentId { get; init; }

        public CreatePostCommand(string title, string body, int authorId, string? tags, int? parentId)
        {
            Title = title;
            Body = body;
            AuthorId = authorId;
            Tags = tags;
            ParentId = parentId;
        }
    }

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result>
    {
        public readonly DatabaseContext _databaseContext;
        public readonly IDateTimeProvider _dateTimeProvider;

        public CreatePostCommandHandler(DatabaseContext databaseContext, IDateTimeProvider dateTimeProvider)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<Result> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var author = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == request.AuthorId, cancellationToken: cancellationToken);
            if (author == null)
                return Errors.User.NotExists(request.AuthorId);

            var createPostResult = Post.Create(request.Title, request.Body, _dateTimeProvider.Now, request.AuthorId, author.DisplayName, request.Tags);
            if (createPostResult.IsFailure)
                return createPostResult.Error;

            _databaseContext.Attach(createPostResult.Value);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
