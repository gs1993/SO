﻿using Logic.Posts.Dtos;
using Logic.Posts.Entities;
using Logic.Utils;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Posts.Queries
{
    public record GetPostQuery : IRequest<PostDetailsDto>
    {
        public int Id { get; init; }
    }

    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostDetailsDto>
    {
        private readonly IReadOnlyDatabaseContext _readOnlyContext;

        public GetPostQueryHandler(IReadOnlyDatabaseContext readOnlyContext)
        {
            _readOnlyContext = readOnlyContext;
        }


        public async Task<PostDetailsDto> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _readOnlyContext.Get<Post>(request.Id);
            if (post == null) 
                return null;

            return new PostDetailsDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                AnswerCount = post.AnswerCount ?? 0,
                CommentCount = post.CommentCount ?? 0,
                CommunityOwnedDate = post.CommunityOwnedDate,
                CreationDate = post.CreationDate,
                FavoriteCount = post.FavoriteCount ?? 0,
                LastEditorDisplayName = post.LastEditorDisplayName,
                Score = post.Score,
                LastActivityDate = post.LastActivityDate,
                LastEditDate = post.LastEditDate,
                ViewCount = post.ViewCount,
                Tags = post.Tags,
                ClosedDate = post.ClosedDate,
                IsClosed = post.ClosedDate != null,
                Comments = post.Comments.Select(c => new CommentDto
                {
                    Text = c.Text,
                    Score = c.Score,
                    CreationDate = c.CreationDate,
                    //UserName = c.
                    //TODO: check UserName
                })
            };
        }
    }
}