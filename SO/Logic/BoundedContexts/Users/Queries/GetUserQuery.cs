using Dawn;
using Logic.BoundedContexts.Users.Dto;
using Logic.Utils.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Users.Queries
{
    public record GetUserQuery : IRequest<UserDetailsDto?>
    {
        public int Id { get; init; }

        public GetUserQuery(int id)
        {
            Id = id;
        }
    }

    public record GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDetailsDto?>
    {
        private readonly ReadOnlyDatabaseContext _readOnlyContext;

        public GetUserQueryHandler(ReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }

        public async Task<UserDetailsDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();
            Guard.Argument(request.Id).Positive();

            var user = await _readOnlyContext.Users
                .FindAsync(request.Id)
                .ConfigureAwait(false);

            return user != null
                ? new UserDetailsDto
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    CreationDate = user.CreateDate,
                    VoteCount = user.VoteSummary.VoteCount,
                    DownVotes = user.VoteSummary.DownVotes,
                    UpVotes = user.VoteSummary.UpVotes,
                    AboutMe = user.ProfileInfo.AboutMe ?? string.Empty,
                    Age = user.ProfileInfo.Age,
                    CreatedPostCount = user.CreatedPostCount,
                    LastAccessDate = user.LastAccessDate,
                    Location = user.ProfileInfo.Location ?? string.Empty,
                    Reputation = user.Reputation,
                    Views = user.Views,
                    WebsiteUrl = user.ProfileInfo.WebsiteUrl ?? string.Empty
                } : null;
        }
    }
}
