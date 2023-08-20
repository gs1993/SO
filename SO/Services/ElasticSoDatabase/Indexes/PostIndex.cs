namespace ElasticSoDatabase.Indexes
{
    internal class PostIndex
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }
        public int Score { get; init; }
        public int AnswerCount { get; init; }
        public int CommentCount { get; init; }
        public DateTime CreateDate { get; init; }
        public int FavoriteCount { get; init; }
        public int ViewCount { get; init; }
        public string Tags { get; set; }
        public bool IsDeleted { get; init; }
    }
}
