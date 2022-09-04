﻿using Logic.BoundedContexts.Posts.Dtos;
using Logic.BoundedContexts.Posts.Entities;
using Logic.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.BoundedContexts.Posts.Queries
{
    public record GetPostsPageQuery : IRequest<IReadOnlyList<PostListDto>>
    {
        public int Offset { get; init; }
        public int Limit { get; init; }

        public GetPostsPageQuery(int offset, int limit)
        {
            Offset = offset;
            Limit = limit;
        }
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
            var posts = await _readOnlyContext
                .GetQuery<Post>()
                .Skip(request.Offset)
                .Take(request.Limit)
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
