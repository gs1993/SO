using Dawn;
using ElasticSoDatabase.Indexes;
using ElasticSoDatabase.Services;
using Logic.Queries.Posts.Dtos;
using Logic.Read.Posts.Dtos;
using Logic.Read.Posts.Queries;
using MediatR;

namespace ElasticSoDatabase.QueryHandlers
{
    internal class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostDetailsDto?>
    {
        private readonly IPostSearchService _postSearchService;

        public GetPostQueryHandler(IPostSearchService postSearchService)
        {
            _postSearchService = postSearchService;
        }

        public async Task<PostDetailsDto?> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            Guard.Argument(request).NotNull();

            var searchArgs = new List<SearchArgs>
            {
                new SearchArgs
                {
                    Field = nameof(PostIndex.Id),
                    Value = request.Id.ToString(),
                    Operation = SearchOperation.Equals
                }
            };

            var sortArgs = new SortArgs
            {
                Field = nameof(PostIndex.CreateDate),
                SortDirection = SortDirection.Descending
            };

            var postListResult = await _postSearchService.Search(
                offset: 0,
                limit: 1,
                searchArgs: searchArgs,
                sortArgs: sortArgs, cancellationToken);

            if (postListResult.IsFailure)
                throw new Exception(postListResult.Error);

            var post = postListResult.Value
                .Items
                .SingleOrDefault();

            if (post is null)
                return null;

            var dto = new PostDetailsDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                CommentCount = post.CommentCount,
                CreationDate = post.CreateDate,
                AnswerCount = post.AnswerCount,
                ViewCount = post.ViewCount,
                Score = post.Score
            };

            return dto;
        }
    }
}