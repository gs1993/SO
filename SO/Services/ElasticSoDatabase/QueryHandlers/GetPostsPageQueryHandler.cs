using Dawn;
using ElasticSoDatabase.Indexes;
using ElasticSoDatabase.Services;
using Logic.Queries.Posts.Dtos;
using Logic.Read.Posts.Dtos;
using Logic.Read.Posts.Queries;
using MediatR;

namespace ElasticSoDatabase.QueryHandlers
{
    internal class GetPostsPageQueryHandler : IRequestHandler<GetPostsPageQuery, PaginatedPostList>
    {
        private readonly IPostSearchService _postSearchService;

        public GetPostsPageQueryHandler(IPostSearchService postSearchService)
        {
            _postSearchService = postSearchService;
        }

        public async Task<PaginatedPostList> Handle(GetPostsPageQuery request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();

            var sortArgs = request.SortArgs ?? new SortArgs
            {
                Field = nameof(PostIndex.CreateDate),
                SortDirection = SortDirection.Descending
            };

            var postListResult = await _postSearchService.Search(
                offset: request.Offset,
                limit: request.Limit, 
                searchArgs: request.SearchArgs, 
                sortArgs: sortArgs, cancellationToken);

            if (postListResult.IsFailure)
                throw new Exception(postListResult.Error);

            var dto = postListResult.Value
                .Items
                .Select(x => new PostListDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Body = x.Body,
                    CommentCount = x.CommentCount,
                    CreationDate = x.CreateDate,
                    AnswerCount = x.AnswerCount,
                    ViewCount = x.ViewCount,
                    Score = x.Score,
                    Tags = Array.Empty<string>(),
                    IsClosed = false
                });

            return new PaginatedPostList
            {
                Posts = dto?.ToList()?.AsReadOnly() ?? new List<PostListDto>().AsReadOnly(),
                Count = postListResult.Value.TotalCount
            };
        }
    }
}