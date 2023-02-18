using CSharpFunctionalExtensions;
using Dawn;
using Logic.Utils;
using Logic.Utils.Db;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Users.Command
{
    public record BanUserCommand : IRequest<Result>
    {
        public int Id { get; init; }

        public BanUserCommand(int id)
        {
            Id = id;
        }
    }

    public class PermaBanCommandHandler : IRequestHandler<BanUserCommand, Result>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PermaBanCommandHandler(DatabaseContext databaseContext, IDateTimeProvider dateTimeProvider)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<Result> Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();
            Guard.Argument(request.Id).Positive();

            var user = await _databaseContext.Users
                .FindAsync(request.Id)
                .ConfigureAwait(false);

            if (user == null || user.IsDeleted)
                return Result.Failure("User not found");

            var result = user.Ban(_dateTimeProvider.Now);
            if (result.IsFailure)
                return result;

            await _databaseContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return Result.Success();
        }
    }
}
