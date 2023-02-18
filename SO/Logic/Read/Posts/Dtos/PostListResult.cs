using System.Collections.Generic;

namespace Logic.Queries.Posts.Dtos
{
    public sealed class PaginatedPostList
    {
        public IReadOnlyList<PostListDto> Posts { get; init; }
        public int Count { get; init; }
    }
}
