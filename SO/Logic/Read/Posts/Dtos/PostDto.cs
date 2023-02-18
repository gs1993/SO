using System;
using System.Collections.Generic;

namespace Logic.Queries.Posts.Dtos
{
    public class PostDto
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime? LastUpdateDate { get; init; }
        public bool IsDeleted { get; init; }
        public string? Title { get; init; }
        public string Body { get; init; }
        public int Score { get; init; }
        public string? Tags { get; init; }
        public int? AcceptedAnswerId { get; init; }
        public int AnswerCount { get; init; }
        public DateTime? ClosedDate { get; init; }
        public int CommentCount { get; init; }
        public DateTime? CommunityOwnedDate { get; init; }
        public int FavoriteCount { get; init; }
        public DateTime LastActivityDate { get; init; }
        public string? LastEditorDisplayName { get; init; }
        public int LastEditorUserId { get; init; }
        public int ViewCount { get; init; }

        public virtual IReadOnlyList<CommentDto> Comments { get; init; }
    }
}
