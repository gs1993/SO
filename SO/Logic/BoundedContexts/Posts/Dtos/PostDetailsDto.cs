using System;
using System.Collections.Generic;

namespace Logic.BoundedContexts.Posts.Dtos
{
    public sealed class PostDetailsDto
    {
        public int Id { get; init; }
        public int AnswerCount { get; init; }
        public string Body { get; init; }
        public DateTime? ClosedDate { get; init; }
        public int CommentCount { get; init; }
        public bool IsClosed { get; init; }
        public DateTime? CommunityOwnedDate { get; init; }
        public DateTime CreationDate { get; init; }
        public int FavoriteCount { get; init; }
        public DateTime LastActivityDate { get; init; }
        public DateTime? LastEditDate { get; init; }
        public string LastEditorDisplayName { get; init; }
        public int Score { get; init; }
        public string Tags { get; init; }
        public string Title { get; init; }
        public int ViewCount { get; init; }

        public IEnumerable<CommentDto> Comments { get; init; }
    }
}
