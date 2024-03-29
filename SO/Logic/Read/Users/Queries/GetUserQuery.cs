﻿using Dawn;
using Logic.BoundedContexts.Users.Dto;
using Logic.Utils.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                .ConfigureAwait(false);

            return user != null
                ? new UserDetailsDto
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    CreationDate = user.CreateDate,
                    DownVotes = user.DownVotes,
                    UpVotes = user.UpVotes,
                    AboutMe = user.AboutMe ?? string.Empty,
                    Age = user.Age,
                    LastAccessDate = user.LastAccessDate,
                    Location = user.Location ?? string.Empty,
                    Reputation = user.Reputation,
                    Views = user.Views,
                    WebsiteUrl = user.WebsiteUrl ?? string.Empty
                } : null;
        }
    }
}
