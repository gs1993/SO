using CSharpFunctionalExtensions;
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
            var user = await _databaseContext.Users.FindAsync(request.UserId);
            if (user == null)
                return Result.Failure("User does not exists");

            var post = await _databaseContext.Posts.FindAsync(request.PostId);
            if (post == null)
                return Result.Failure("Post does not exists");

            _databaseContext.Entry(post).Collection(x => x.Votes).Load();

            var result = post.DownVote(user);
            if (result.IsFailure)
                return result;

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    }
}
