using System;

namespace Logic.BoundedContexts.Posts.Dtos
{
    public sealed class CommentDto
    {
        public int Id { get; init; }
        public DateTime CreationDate { get; init; }
        public int? Score { get; init; }
        public string Text { get; init; }
        public string UserName { get; init; }
    }
}
