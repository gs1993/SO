﻿using Dawn;
using Logic.BoundedContexts.Posts.Dtos;
using Logic.Utils.Db;
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

            var posts = await _readOnlyContext.Posts
                .OrderByDescending(x => x.Id)
                .Take(request.Size)
                .Select(x => new PostListDto
                {
                    Id = x.Id,
                    Title = x.Title ?? string.Empty,
                    Body = x.Body,
                    AnswerCount = x.AnswerCount,
                    CommentCount = x.CommentCount,
                    Score = x.Score,
                    ViewCount = x.ViewCount,
                    CreationDate = x.CreateDate,
                    IsClosed = x.ClosedDate != null,
                    Tags = x.GetTagsArray()
                }).ToListAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return posts ?? new List<PostListDto>();
        }
    }
}
