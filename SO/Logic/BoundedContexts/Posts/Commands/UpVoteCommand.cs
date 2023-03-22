using CSharpFunctionalExtensions;
using Dawn;
using Logic.Utils;
using Logic.Utils.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            Guard.Argument(request).NotNull();
            Guard.Argument(request.UserId).Positive();
            Guard.Argument(request.PostId).Positive();

            var user = await _databaseContext.Users
                .FindByIdAsync(request.UserId, cancellationToken)
                .ConfigureAwait(false);

            if (user == null)
                return Errors.Users.DoesNotExists(request.UserId);

            var post = await _databaseContext.Posts
                .Include(x => x.Votes)
                .FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken)
                .ConfigureAwait(false);

            if (post == null)
                return Errors.Posts.DoesNotExists(request.PostId);
            
            var result = post.UpVote(user);
            if (result.IsFailure)
                return result;

            await _databaseContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();

        }
    }
}
