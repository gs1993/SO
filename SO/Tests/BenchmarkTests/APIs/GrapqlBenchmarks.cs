using BenchmarkDotNet.Attributes;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Logic.BoundedContexts.Posts.Dtos;
using GraphQL.Client.Abstractions;

namespace BenchmarkTests.APIs
{
    public class GrapqlBenchmarks
    {
        private static readonly GraphQLHttpClient _graphqlClient = new("http://localhost:5000/graphql", new NewtonsoftJsonSerializer());

        [Benchmark]
        public async Task<IReadOnlyList<PostListDto>> GrapqlPostGet100()
        {
            var request = new GraphQLRequest
            {
                Query = @"query { postsPage (offset: 100, limit: 100){ id answerCount isClosed commentCount creationDate score title shortBody viewCount }}"
            };

            var result = await _graphqlClient
                .SendQueryAsync(request, () => new { postsPage = new List<PostListDto>()})
                .ConfigureAwait(false);

            return result.Data?.postsPage;
        }

        [Benchmark]
        public async Task<IReadOnlyList<PostListDto>> GrapqlPostGet1000()
        {
            var request = new GraphQLRequest
            {
                Query = @"query { postsPage (offset: 100, limit: 100){ id answerCount isClosed commentCount creationDate score title shortBody viewCount }}"
            };

            var result = await _graphqlClient
                .SendQueryAsync(request, () => new { postsPage = new List<PostListDto>() })
                .ConfigureAwait(false);

            return result.Data?.postsPage;
        }

        [Benchmark]
        public async Task<IReadOnlyList<PostListDto>> GrapqlPostGet10000()
        {
            var request = new GraphQLRequest
            {
                Query = @"query { postsPage (offset: 100, limit: 100){ id answerCount isClosed commentCount creationDate score title shortBody viewCount }}"
            };

            var result = await _graphqlClient
                .SendQueryAsync(request, () => new { postsPage = new List<PostListDto>() })
                .ConfigureAwait(false);

            return result.Data?.postsPage;
        }
    }
}
