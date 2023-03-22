using Dawn;
using Logic.Queries.Posts.Dtos;
using Logic.Utils.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Read.Posts.Queries
{
    public record GetPostsPageQuery : IRequest<PaginatedPostList>
    {
        public int Offset { get; init; }
        public int Limit { get; init; }

        public GetPostsPageQuery(int offset, int limit)
        {
            Offset = offset;
            Limit = limit;
        }
    }

    public class GetPostsPageQueryHandler : IRequestHandler<GetPostsPageQuery, PaginatedPostList>
    {
        private readonly ReadOnlyDatabaseContext _readOnlyContext;

        public GetPostsPageQueryHandler(ReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext ?? throw new ArgumentNullException(nameof(readOnlyContext));
        }


        public async Task<PaginatedPostList> Handle(GetPostsPageQuery request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();
            Guard.Argument(request.Offset).NotNegative();
            Guard.Argument(request.Limit).Positive();

            var query = _readOnlyContext.Posts
                .AsQueryable();

            int count = await query.CountAsync(cancellationToken);

            var posts = await query
                .Include(x => x.User)
                .Skip(request.Offset)
                .Take(request.Limit)
                .Select(x => new PostListDto(x))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return new PaginatedPostList
            {
                Posts = posts ?? new List<PostListDto>(),
                Count = count
            };
        }
    }
}
