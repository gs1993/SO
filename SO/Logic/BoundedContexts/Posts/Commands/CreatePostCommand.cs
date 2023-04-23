using CSharpFunctionalExtensions;
using Dawn;
using Logic.BoundedContexts.Posts.Entities;
using Logic.Contracts;
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
        public readonly IMediator _mediator;

        public CreatePostCommandHandler(DatabaseContext databaseContext, IDateTimeProvider dateTimeProvider, IMediator mediator)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _mediator = mediator;
        }

        public async Task<Result> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();
            Guard.Argument(request.AuthorId).Positive();

            var author = await _databaseContext.Users
                .FindByIdAsync(request.AuthorId, cancellationToken)
                .ConfigureAwait(false);

            if (author == null)
                return Errors.Users.DoesNotExists(request.AuthorId);

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

            var spamValidationResult = await _mediator.Send(new ValidatePostContentCommand(request.Body), cancellationToken);
            if (spamValidationResult.IsSuccess && spamValidationResult.Value == IsSpamPredictionEnum.Spam)
                return Errors.Posts.InappropriatePostContent();

            _databaseContext.Attach(createPostResult.Value);

            await _databaseContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }
    }    
}
