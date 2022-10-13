using Logic.BoundedContexts.Posts.Dtos;
using Logic.Utils.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Queries
{
    public record GetPostQuery : IRequest<PostDetailsDto?>
    {
        public int Id { get; init; }

        public GetPostQuery(int id)
        {
            Id = id;
        }
    }

    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostDetailsDto?>
    {
        private readonly ReadOnlyDatabaseContext _readOnlyContext;

        public GetPostQueryHandler(ReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }


        public async Task<PostDetailsDto?> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _readOnlyContext.Posts
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            if (post == null)
                return null;

            return new PostDetailsDto
            {
                Id = post.Id,
                Title = post.Title ?? string.Empty,
                Body = post.Body,
                AnswerCount = post.AnswerCount,
                CommentCount = post.CommentCount,
                CommunityOwnedDate = post.CommunityOwnedDate,
                CreationDate = post.CreateDate,
                FavoriteCount = post.FavoriteCount,
                LastEditorDisplayName = post.LastEditorDisplayName ?? string.Empty,
                Score = post.Score,
                LastActivityDate = post.LastActivityDate,
                LastEditDate = post.LastUpdateDate,
                ViewCount = post.ViewCount,
                Tags = post.Tags ?? string.Empty,
                ClosedDate = post.ClosedDate,
                IsClosed = post.ClosedDate != null,
                Comments = post.Comments.Select(c => new CommentDto
                {
                    Text = c.Text,
                    Score = c.Score,
                    CreationDate = c.CreateDate
                })
            };
        }
    }
}
