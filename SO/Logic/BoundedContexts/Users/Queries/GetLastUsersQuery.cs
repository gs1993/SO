using Logic.BoundedContexts.Users.Dto;
using Logic.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Users.Queries
{
    public record GetLastUsersQuery : IRequest<IReadOnlyList<LastUserDto>>
    {
        public int Size { get; init; }
    }

    public class GetLastUsersQueryHandler : IRequestHandler<GetLastUsersQuery, IReadOnlyList<LastUserDto>>
    {
        private readonly IReadOnlyDatabaseContext _readOnlyContext;

        public GetLastUsersQueryHandler(IReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }

        public Task<IReadOnlyList<LastUserDto>> Handle(GetLastUsersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
