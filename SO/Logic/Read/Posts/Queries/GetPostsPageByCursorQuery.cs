using Dawn;
using Logic.Queries.Posts.Dtos;
using Logic.Utils.Db;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Logic.Read.Posts.Queries
{
    public record GetPostsPageByCursorQuery : IRequest<PaginatedPostList>
    {
        public int? Cursor { get; init; }
        public int Limit { get; init; }

        public GetPostsPageByCursorQuery(int? cursor, int limit)
        {
            Cursor = cursor;
            Limit = limit;
        }
    }

    public class GetPostsPageByCursorQueryHandler : IRequestHandler<GetPostsPageByCursorQuery, PaginatedPostList>
    {
        private readonly ReadOnlyDatabaseContext _readOnlyContext;

        public GetPostsPageByCursorQueryHandler(ReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext ?? throw new ArgumentNullException(nameof(readOnlyContext));
        }


        public async Task<PaginatedPostList> Handle(GetPostsPageByCursorQuery request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();
            Guard.Argument(request.Cursor).Positive();
            Guard.Argument(request.Limit).Positive();

            var query = _readOnlyContext.Posts
                .AsQueryable();

            int count = await query.CountAsync(cancellationToken);

            var posts = await query
                .OrderBy(x => x.Id)
                .Where(x => request.Cursor == null || x.Id > request.Cursor)
                .Include(x => x.User)
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
