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
            Guard.Argument(request).NotNull();
            Guard.Argument(request.Id).Positive();

            var post = await _databaseContext.Posts
                .FindByIdAsync(request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (post == null)
                return Errors.Posts.DoesNotExists(request.Id);

            var result = post.Close(_dateTimeProvider.Now);
            if (result.IsFailure)
                return result;

            await _databaseContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }
    }
}
