using Logic.Posts.Dtos;
using Logic.Posts.Entities;
using Logic.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Posts.Queries
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
                .OrderByDescending(x => x.CreateDate)
                .Take(request.Size)
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
