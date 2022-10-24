using Dawn;
using Logic.BoundedContexts.Users.Dto;
using Logic.Utils.Db;
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

    public class GetLastUsersQueryHandler : IRequestHandler<GetLastUsersQuery, IReadOnlyList<LastUserDto>>
    {
        private readonly ReadOnlyDatabaseContext _readOnlyContext;

        public GetLastUsersQueryHandler(ReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }

        public async Task<IReadOnlyList<LastUserDto>> Handle(GetLastUsersQuery request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();
            Guard.Argument(request.Size).Positive();

            return await _readOnlyContext.Users
                .OrderByDescending(x => x.CreateDate)
                .Take(request.Size)
                .Select(x => new LastUserDto 
                { 
                    Id = x.Id,
                    DisplayName = x.DisplayName
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
