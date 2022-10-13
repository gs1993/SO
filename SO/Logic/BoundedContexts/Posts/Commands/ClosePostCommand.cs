using CSharpFunctionalExtensions;
using Logic.Utils;
using Logic.Utils.Db;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Commands
{
    public record ClosePostCommand : IRequest<Result>
    {
        public int Id { get; init; }

        public ClosePostCommand(int id)
        {
            Id = id;
        }
    }

    public class ClosePostCommandHandler : IRequestHandler<ClosePostCommand, Result>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ClosePostCommandHandler(DatabaseContext databaseContext, IDateTimeProvider dateTimeProvider)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<Result> Handle(ClosePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _databaseContext.Posts.FindAsync(request.Id);
            if (post == null)
                return Result.Failure("Post does not exist");

            var result = post.Close(_dateTimeProvider.Now);
            if (result.IsFailure)
                return result;

            await _databaseContext.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
