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
    public record GetLastestPostsQuery : IRequest<IReadOnlyList<PostListDto>>
    {
        public int Size { get; init; }

        public GetLastestPostsQuery(int size)
        {
            Size = size;
        }
    }

    public class GetLastestPostsQueryHandler : IRequestHandler<GetLastestPostsQuery, IReadOnlyList<PostListDto>>
    {
        private readonly ReadOnlyDatabaseContext _readOnlyContext;

        public GetLastestPostsQueryHandler(ReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }

        public async Task<IReadOnlyList<PostListDto>> Handle(GetLastestPostsQuery request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();
            Guard.Argument(request.Size).Positive();

            var postList = new List<PostListDto>();
            await foreach (var post in GetPosts(_readOnlyContext, request.Size))
            {
                postList.Add(post);
            }

            return postList.AsReadOnly();
        }

        private static readonly Func<ReadOnlyDatabaseContext, int, IAsyncEnumerable<PostListDto>> GetPosts =
            EF.CompileAsyncQuery((ReadOnlyDatabaseContext context, int size) =>
                context.Set<PostModel>()
                    .Include(x => x.User)
                    .OrderByDescending(x => x.Id)
                    .Take(size)
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
