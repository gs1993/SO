using System;

namespace Logic.Queries.Posts.Dtos
{
    public sealed class PostListDto
    {
        public int Id { get; init; }
        public int AnswerCount { get; init; }
        public bool IsClosed { get; init; }
        public int CommentCount { get; init; }
        public DateTime CreationDate { get; init; }
        public int Score { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }
        public int ViewCount { get; init; }
        public string[] Tags { get; init; }
        public string UserName { get; init; }
    }
}
