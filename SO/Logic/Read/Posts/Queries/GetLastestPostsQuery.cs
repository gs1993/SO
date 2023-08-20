using Dawn;
using Logic.Queries.Posts.Dtos;
using MediatR;
using System.Collections.Generic;

namespace Logic.Read.Posts.Queries
{
    public record GetLastestPostsQuery : IRequest<IReadOnlyList<PostListDto>>
    {
        public int Size { get; init; }

        public GetLastestPostsQuery(int size)
        {
            Guard.Argument(size).Positive();

            Size = size;
        }
    }
}
