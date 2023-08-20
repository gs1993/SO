using Nest;

namespace SqlToElaticMigratorService.Models.v1
{
    internal class PostIndex
    {
        public PostIndex(Post post)
        {
            Id = post.Id;
            AnswerCount = post.AnswerCount;
            Body = post.Body;
            ClosedDate = post.ClosedDate ?? default;
            CommentCount = post.CommentCount;
            CommunityOwnedDate = post.CommunityOwnedDate ?? default;
            CreateDate = post.CreateDate;
            FavoriteCount = post.FavoriteCount;
            IsDeleted = post.IsDeleted;
            LastActivityDate = post.LastActivityDate;
            LastEditorDisplayName = post.LastEditorDisplayName ?? string.Empty;
            Score = post.Score;
            ViewCount = post.ViewCount;
            Title = post.Title ?? string.Empty;

            Tags = !string.IsNullOrWhiteSpace(post.Tags)
                ? post.Tags.Split(' ')
                : Array.Empty<string>();
        }

        public int Id { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }
        public int Score { get; init; }
        public IEnumerable<string> Tags { get; init; }
        public int AnswerCount { get; init; }
        public DateTime ClosedDate { get; init; }
        public int CommentCount { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime CommunityOwnedDate { get; init; }
        public int FavoriteCount { get; init; }
        public DateTime LastActivityDate { get; init; }
        public string LastEditorDisplayName { get; init; }
        public int ViewCount { get; init; }
        public bool IsDeleted { get; init; }

        public static ElasticClient CreateElasticClient(string url, string indexName)
        {
            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(indexName)
                .DefaultMappingFor<PostIndex>(m => m
                    .IndexName(indexName)
                    .IdProperty(post => post.Id));

            var client = new ElasticClient(settings);

            client.Indices.Create(indexName, c => c
                .Map<PostIndex>(m => m
                    .AutoMap()
                    .Properties(ps => ps
                        .Keyword(t => t.Name(n => n.Tags)))));

            return client;
        }
    }
}
