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
using Logic.Read.Posts.Models;

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
            Guard.Argument(request.Cursor).NotNegative();
            Guard.Argument(request.Limit).Positive();

            var posts = new List<PostListDto>();
            await foreach (var post in GetPosts(_readOnlyContext, request.Cursor, request.Limit))
            {
                posts.Add(post);
            }

            int count = await _readOnlyContext.Posts
                .CountAsync(cancellationToken);

            return new PaginatedPostList
            {
                Posts = posts ?? new List<PostListDto>(),
                Count = count
            };
        }

        private static readonly Func<ReadOnlyDatabaseContext, int?, int, IAsyncEnumerable<PostListDto>> GetPosts =
            EF.CompileAsyncQuery((ReadOnlyDatabaseContext context, int? cursor, int limit) =>
                context.Set<PostModel>()
                    .OrderBy(x => x.Id)
                    .Where(x => cursor == null || x.Id > cursor)
                    .Include(x => x.User)
                    .Take(limit)
                    .Select(post => new PostListDto
                    {
                        Id = post.Id,
                        Title = post.Title ?? string.Empty,
                        Body = post.Body,
                        AnswerCount = post.AnswerCount,
                        CommentCount = post.CommentCount,
                        Score = post.Score,
                        ViewCount = post.ViewCount,
                        CreationDate = post.CreateDate,
                        IsClosed = post.ClosedDate != null,
                        Tags = post.GetTagsArray(),
                        UserName = post.User != null ? post.User.DisplayName : string.Empty,
                    }));
    }
}
