using Logic.BoundedContexts.Users.Dto;
using Logic.BoundedContexts.Users.Entities;
using Logic.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Users.Queries
{
    public record GetLastUsersQuery : IRequest<IReadOnlyList<LastUserDto>>
    {
        public int Size { get; init; }

        public GetLastUsersQuery(int size)
        {
            Size = size;
        }
    }

    public class GetLastUsersQueryHandler : IRequestHandler<GetLastUsersQuery?, IReadOnlyList<LastUserDto>>
    {
        private readonly IReadOnlyDatabaseContext _readOnlyContext;

        public GetLastUsersQueryHandler(IReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }

        public async Task<IReadOnlyList<LastUserDto>> Handle(GetLastUsersQuery request, CancellationToken cancellationToken)
        {
            if (request.Size <= 0 || request.Size > 1000)
                throw new ArgumentException($"Invalid size: {request.Size}", nameof(request.Size));

            return await _readOnlyContext
                .GetQuery<User>()
                .OrderByDescending(x => x.CreateDate)
                .Take(request.Size)
                .Select(x => new LastUserDto 
                { 
                    Id = x.Id,
                    DisplayName = x.DisplayName
                })
                .ToListAsync(cancellationToken);
        }
    }
}
