using CSharpFunctionalExtensions;
using Dawn;
using Logic.Utils;
using Logic.Utils.Db;
using Logic.Utils.Db;
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

        public AddCommentCommand(int postId, int userId, string comment)
        {
            PostId = postId;
            UserId = userId;
            Comment = comment;
        }
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
            Guard.Argument(request).NotNull();
            Guard.Argument(request.UserId).Positive();
            Guard.Argument(request.PostId).Positive();

            var user = await _databaseContext.Users
                .FindByIdAsync(request.UserId, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (user == null)
                return Errors.Users.DoesNotExists(request.UserId);

            var post = await _databaseContext.Posts
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken)
                .ConfigureAwait(false);

            if (post == null)
                return Errors.Posts.DoesNotExists(request.PostId);

            var result = post.AddComment(user, request.Comment);
            if (result.IsFailure)
                return result;

            await _databaseContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }
    }
}
