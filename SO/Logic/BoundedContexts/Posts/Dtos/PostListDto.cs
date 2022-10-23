using System;

namespace Logic.BoundedContexts.Posts.Dtos
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
        public string ShortBody { get; init; }
        public int ViewCount { get; init; }
    }
}
