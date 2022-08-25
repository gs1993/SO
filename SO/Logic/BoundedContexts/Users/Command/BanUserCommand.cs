﻿using CSharpFunctionalExtensions;
using Logic.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Users.Command
{
    public record BanUserCommand : IRequest<Result>
    {
        public int Id { get; init; }
    }

    public class PermaBanCommandHandler : IRequestHandler<BanUserCommand, Result>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PermaBanCommandHandler(DatabaseContext databaseContext, IDateTimeProvider dateTimeProvider)
        {
            _databaseContext = databaseContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            if (user == null || user.IsDeleted)
                return Result.Failure("User not found");

            var result = user.Ban(_dateTimeProvider.Now);
            if (result.IsFailure)
                return result;

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
