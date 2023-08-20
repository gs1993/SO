using Dawn;
using Logic.Queries.Posts.Dtos;
using Logic.Read.Posts.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Read.Posts.Queries
{
    public record GetPostsPageQuery : IRequest<PaginatedPostList>
    {
        public int Offset { get; init; }
        public int Limit { get; init; }
        public IReadOnlyList<SearchArgs> SearchArgs { get; init; }
        public SortArgs? SortArgs { get; init; }

        public GetPostsPageQuery(int offset, int limit, IEnumerable<SearchArgs> searchArgs, SortArgs? sortArgs)
        {
            Guard.Argument(offset).NotNegative();
            Guard.Argument(limit).Positive();

            Offset = offset;
            Limit = limit;
            SearchArgs = searchArgs?
                .ToList()?
                .AsReadOnly() 
                ?? new List<SearchArgs>().AsReadOnly();
            SortArgs = sortArgs;
        }
    }
}
