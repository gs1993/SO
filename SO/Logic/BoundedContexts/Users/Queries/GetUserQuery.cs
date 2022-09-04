﻿using Logic.BoundedContexts.Users.Dto;
using Logic.BoundedContexts.Users.Entities;
using Logic.Utils;
using MediatR;
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
        private readonly IReadOnlyDatabaseContext _readOnlyContext;

        public GetUserQueryHandler(IReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }

        public async Task<UserDetailsDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new ArgumentException($"Invalid id: {request.Id}", nameof(request.Id));

            var user = await _readOnlyContext.Get<User>(request.Id);
            return user != null
                ? new UserDetailsDto
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    CreationDate = user.CreateDate,
                    VoteCount = user.VoteSummary.VoteCount,
                    DownVotes = user.VoteSummary.DownVotes,
                    UpVotes = user.VoteSummary.UpVotes,
                    AboutMe = user.AboutMe ?? string.Empty,
                    Age = user.Age,
                    CreatedPostCount = user.CreatedPostCount,
                    LastAccessDate = user.LastAccessDate,
                    Location = user.Location ?? string.Empty,
                    Reputation = user.Reputation,
                    Views = user.Views,
                    WebsiteUrl = user.WebsiteUrl ?? string.Empty
                } : null;
        }
    }
}
