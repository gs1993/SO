using CSharpFunctionalExtensions;
using Logic.Utils.Db;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Commands
{
    public record UpVoteCommand : IRequest<Result>
    {
        public int PostId { get; init; }
        public int UserId { get; init; }

        public UpVoteCommand(int postId, int userId)
        {
            PostId = postId;
            UserId = userId;
        }
    }

    public class UpVoteCommandHandler : IRequestHandler<UpVoteCommand, Result>
    {
        public readonly DatabaseContext _databaseContext;

        public UpVoteCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        }

        public async Task<Result> Handle(UpVoteCommand request, CancellationToken cancellationToken)
        {
            var user = await _databaseContext.Users.FindAsync(request.UserId);
            if (user == null)
                return Result.Failure("User does not exists");

            var post = await _databaseContext.Posts.FindAsync(request.PostId);
            if (post == null)
                return Result.Failure("Post does not exists");

            await _databaseContext.Entry(post).Collection(x => x.Votes).LoadAsync(cancellationToken);
            
            var result = post.UpVote(user);
            if (result.IsFailure)
                return result;

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    }
}
