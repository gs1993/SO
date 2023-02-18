using Logic.Read.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Read.Posts.Models
{
    public sealed class PostModel
    {
        public int Id { get; init; }
        public bool IsDeleted { get; init; }
        public int AnswerCount { get; init; }
        public string Body { get; init; }
        public DateTime? ClosedDate { get; init; }
        public int CommentCount { get; init; }
        public DateTime? CommunityOwnedDate { get; init; }
        public DateTime CreateDate { get; init; }
        public int FavoriteCount { get; init; }
        public DateTime LastActivityDate { get; init; }
        public DateTime? LastUpdateDate { get; init; }
        public string LastEditorDisplayName { get; init; }
        public int Score { get; init; }
        public string? Tags { get; init; }
        public string? Title { get; init; }
        public int ViewCount { get; init; }

        public UserModel User { get; init; }
        public IReadOnlyList<CommentModel> Comments { get; init; }

        public string[] GetTagsArray()
        {
            if (string.IsNullOrWhiteSpace(Tags))
                return Array.Empty<string>();

            return Tags[1..^1]
                .Split("><")
                .OrderBy(x => x)
                .ToArray();
        }
    }
}
