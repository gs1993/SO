using CSharpFunctionalExtensions;
using Logic.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Commands
{
    public record ClosePostCommand : IRequest<Result>
    {
        public int Id { get; init; }
    }

    public class ClosePostCommandHandler : IRequestHandler<ClosePostCommand, Result>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ClosePostCommandHandler(DatabaseContext databaseContext, IDateTimeProvider dateTimeProvider)
        {
            _databaseContext = databaseContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(ClosePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _databaseContext.Posts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (post == null)
                return Result.Failure("Post does not exist");

            var result = post.Close(_dateTimeProvider.Now);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
