using Dawn;
using Logic.Queries.Posts.Dtos;
using MediatR;

namespace Logic.Read.Posts.Queries
{
    public record GetPostQuery : IRequest<PostDetailsDto?>
    {
        public int Id { get; init; }

        public GetPostQuery(int id)
        {
            Guard.Argument(id).Positive();

            Id = id;
        }
    }
}
