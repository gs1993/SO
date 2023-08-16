using CdcWorkerService.Db.Models;

namespace CdcWorkerService.EsClient
{
    internal class PostIndex
    {
        public PostIndex()
        {
        }

        public PostIndex(PostCT cdcPost)
        {
            Id = cdcPost.Id;
            AnswerCount = cdcPost.AnswerCount ?? default;
            Body = cdcPost.Body ?? string.Empty;
            ClosedDate = cdcPost.ClosedDate ?? default;
            CommentCount = cdcPost.CommentCount ?? default;
            CommunityOwnedDate = cdcPost.CommunityOwnedDate ?? default;
            CreateDate = cdcPost.CreateDate ?? default;
            FavoriteCount = cdcPost.FavoriteCount ?? default;
            IsDeleted = cdcPost.IsDeleted ?? default;
            LastActivityDate = cdcPost.LastActivityDate ?? default;
            LastEditorDisplayName = cdcPost.LastEditorDisplayName ?? string.Empty;
            Score = cdcPost.Score ?? default;
            ViewCount = cdcPost.ViewCount ?? default;
            Title = cdcPost.Title ?? string.Empty;

            Tags = !string.IsNullOrWhiteSpace(cdcPost.Tags)
                ? cdcPost.Tags.Split(' ')
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
    }
}
