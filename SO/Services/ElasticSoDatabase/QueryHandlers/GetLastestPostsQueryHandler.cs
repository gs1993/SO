using Dawn;
using ElasticSoDatabase.Indexes;
using ElasticSoDatabase.Services;
using Logic.Queries.Posts.Dtos;
using Logic.Read.Posts.Dtos;
using Logic.Read.Posts.Queries;
using MediatR;

namespace ElasticSoDatabase.QueryHandlers
{
    internal class GetLastestPostsQueryHandler : IRequestHandler<GetLastestPostsQuery, IReadOnlyList<PostListDto>>
    {
        private readonly IPostSearchService _postSearchService;

        public GetLastestPostsQueryHandler(IPostSearchService postSearchService)
        {
            _postSearchService = postSearchService;
        }

        public async Task<IReadOnlyList<PostListDto>> Handle(GetLastestPostsQuery request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();

            var sortArgs = new SortArgs
            {
                Field = nameof(PostIndex.CreateDate),
                SortDirection = SortDirection.Descending
            };

            var postListResult = await _postSearchService.Search(
                offset: 0,
                limit: request.Size,
                searchArgs: Array.Empty<SearchArgs>(),
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

            return dto?.ToList()?.AsReadOnly() 
                ?? new List<PostListDto>().AsReadOnly();
        }
    }
}