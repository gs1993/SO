using Logic.BoundedContexts.Users.Dto;
using Logic.Utils;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Users.Queries
{
    public record GetUserQuery : IRequest<UserDetailsDto>
    {
        public int Id { get; init; }
    }

    public record GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDetailsDto>
    {
        private readonly IReadOnlyDatabaseContext _readOnlyContext;

        public GetUserQueryHandler(IReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }

        public Task<UserDetailsDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
