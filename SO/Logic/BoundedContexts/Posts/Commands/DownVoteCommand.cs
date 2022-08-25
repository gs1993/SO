using CSharpFunctionalExtensions;
using Logic.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Commands
{
    public record DownVoteCommand : IRequest<Result>
    {
        public int PostId { get; init; }
        public int UserId { get; init; }
    }

    public class DownVoteCommandHandler : IRequestHandler<DownVoteCommand, Result>
    {
        public readonly DatabaseContext _databaseContext;

        public DownVoteCommandHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Result> Handle(DownVoteCommand request, CancellationToken cancellationToken)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken);
            if (user == null)
                return Result.Failure("User does not exists");

            var post = await _databaseContext.Posts.FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken);
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
