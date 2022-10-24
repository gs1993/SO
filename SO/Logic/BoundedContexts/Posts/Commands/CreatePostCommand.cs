using CSharpFunctionalExtensions;
using Dawn;
using Logic.BoundedContexts.Posts.Entities;
using Logic.Utils;
using Logic.Utils.Db;
using MediatR;
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
            Guard.Argument(request).NotNull();
            Guard.Argument(request.AuthorId).Positive();

            var author = await _databaseContext.Users
                .FindAsync(request.AuthorId)
                .ConfigureAwait(false);

            if (author == null)
                return Errors.User.DoesNotExists(request.AuthorId);

            var createPostResult = Post.Create
            (
                title: request.Title, 
                body: request.Body, 
                createDate: _dateTimeProvider.Now, 
                authorId: request.AuthorId,
                authorName: author.DisplayName, 
                tags: request.Tags
            );

            if (createPostResult.IsFailure)
                return createPostResult.Error;

            _databaseContext.Attach(createPostResult.Value);

            await _databaseContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }
    }
}
