using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Posts.Commands
{
    public record ClosePostCommand : IRequest<Result>
    {
        public int Id { get; init; }
    }

    public class ClosePostCommandHandler : IRequestHandler<ClosePostCommand, Result>
    {
        public Task<Result> Handle(ClosePostCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
