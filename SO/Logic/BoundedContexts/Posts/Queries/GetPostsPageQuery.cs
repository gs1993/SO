using Logic.BoundedContexts.Posts.Dtos;
using Logic.BoundedContexts.Posts.Entities;
using Logic.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Queries
{
    public record GetPostsPageQuery : IRequest<IReadOnlyList<PostListDto>>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }

    public class GetPostsPageQueryHandler : IRequestHandler<GetPostsPageQuery, IReadOnlyList<PostListDto>>
    {
        private readonly IReadOnlyDatabaseContext _readOnlyContext;

        public GetPostsPageQueryHandler(IReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext ?? throw new ArgumentNullException(nameof(readOnlyContext));
        }


        public async Task<IReadOnlyList<PostListDto>> Handle(GetPostsPageQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
            var posts = await _readOnlyContext
                .GetQuery<Post>()
                .Skip(skip)
                .Take(request.PageSize)
                .Select(x => new PostListDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortBody = x.Body.Substring(0, 150),
                    AnswerCount = x.AnswerCount ?? 0,
                    CommentCount = x.CommentCount ?? 0,
                    Score = x.Score,
                    ViewCount = x.ViewCount,
                    CreationDate = x.CreateDate,
                    IsClosed = x.ClosedDate != null
                }).ToListAsync(cancellationToken: cancellationToken);

            return posts ?? new List<PostListDto>();
        }
    }
}
