using CSharpFunctionalExtensions;
using Dawn;
using Logic.Utils;
using Logic.Utils.Db;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Commands
{
    public record DownVoteCommand : IRequest<Result>
    {
        public int PostId { get; init; }
        public int UserId { get; init; }

        public DownVoteCommand(int postId, int userId)
        {
            PostId = postId;
            UserId = userId;
        }
    }

    public class DownVoteCommandHandler : IRequestHandler<DownVoteCommand, Result>
    {
        public readonly DatabaseContext _databaseContext;

        public DownVoteCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        }

        public async Task<Result> Handle(DownVoteCommand request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();
            Guard.Argument(request.UserId).Positive();
            Guard.Argument(request.PostId).Positive();

            var user = await _databaseContext.Users
                .FindAsync(request.UserId)
                .ConfigureAwait(false);

            if (user == null)
                return Errors.User.DoesNotExists(request.UserId);

            var post = await _databaseContext.Posts
                .FindAsync(request.PostId)
                .ConfigureAwait(false);

            if (post == null)
                return Errors.Post.DoesNotExists(request.PostId);

            _databaseContext.Entry(post).Collection(x => x.Votes).Load();

            var result = post.DownVote(user);
            if (result.IsFailure)
                return result;

            await _databaseContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();

        }
    }
}
