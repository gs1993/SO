using Dawn;
using Logic.Queries.Posts.Dtos;
using Logic.Read.Posts.Models;
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

            var posts = new List<PostListDto>();
            await foreach (var post in GetPosts(_readOnlyContext, request.Offset, request.Limit))
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

        private static readonly Func<ReadOnlyDatabaseContext, int, int, IAsyncEnumerable<PostListDto>> GetPosts =
            EF.CompileAsyncQuery((ReadOnlyDatabaseContext context, int offset, int limit) =>
                context.Set<PostModel>()
                    .Include(x => x.User)
                    .OrderBy(x => x.Id)
                    .Skip(offset)
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
