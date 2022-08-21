using Logic.BoundedContexts.Posts.Dtos;
using Logic.BoundedContexts.Posts.Entities;
using Logic.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Queries
{
    public record GetLastestPostsQuery : IRequest<IReadOnlyList<PostListDto>>
    {
        public int Size { get; init; }
    }

    public class GetLastestPostsQueryHandler : IRequestHandler<GetLastestPostsQuery, IReadOnlyList<PostListDto>>
    {
        private readonly IReadOnlyDatabaseContext _readOnlyContext;

        public GetLastestPostsQueryHandler(IReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }

        public async Task<IReadOnlyList<PostListDto>> Handle(GetLastestPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _readOnlyContext
                .GetQuery<Post>()
                .OrderByDescending(x => x.Id)
                .Take(request.Size)
                .Select(x => new PostListDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    ShortBody = x.Body.Substring(0, 150),
                    AnswerCount = x.AnswerCount,
                    CommentCount = x.CommentCount,
                    Score = x.Score,
                    ViewCount = x.ViewCount,
                    CreationDate = x.CreateDate,
                    IsClosed = x.ClosedDate != null
                }).ToListAsync(cancellationToken: cancellationToken);

            return posts ?? new List<PostListDto>();
        }
    }
}
