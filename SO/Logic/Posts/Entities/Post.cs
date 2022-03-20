using CSharpFunctionalExtensions;
using Logic.Models;
using System;
using System.Collections.Generic;

namespace Logic.Posts.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public int Score { get; private set; }
        public string Tags { get; private set; }
        public int? AcceptedAnswerId { get; private set; }
        public int? AnswerCount { get; private set; }
        public DateTime? ClosedDate { get; private set; }
        public int? CommentCount { get; private set; }
        public DateTime? CommunityOwnedDate { get; private set; }
        public int? FavoriteCount { get; private set; }
        public DateTime LastActivityDate { get; private set; }
        public DateTime? LastEditDate { get; private set; }
        public string LastEditorDisplayName { get; private set; }
        public int? LastEditorUserId { get; private set; }
        public int? OwnerUserId { get; private set; }
        public int? ParentId { get; private set; }
        public int ViewCount { get; private set; }

        public virtual PostType PostType { get; private set; }

        private readonly List<Comment> _comments;
        public virtual IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

        private readonly List<Vote> _votes;
        public virtual IReadOnlyList<Vote> Votes => _votes.AsReadOnly();

        private readonly List<PostLink> _postLinks;
        public virtual IReadOnlyList<PostLink> PostLinks => _postLinks.AsReadOnly();

        protected Post() { }
        public Post(string title, string body, int score, string tags, int? acceptedAnswerId, int? answerCount, 
            DateTime? closedDate, int? commentCount, DateTime? communityOwnedDate, DateTime createDate, 
            int? favoriteCount, DateTime lastActivityDate, DateTime? lastEditDate, string lastEditorDisplayName, 
            int? lastEditorUserId, int? ownerUserId, int? parentId, int viewCount)
        {
            Title = title;
            Body = body;
            Score = score;
            Tags = tags;
            AcceptedAnswerId = acceptedAnswerId;
            AnswerCount = answerCount;
            ClosedDate = closedDate;
            CommentCount = commentCount;
            CommunityOwnedDate = communityOwnedDate;
            FavoriteCount = favoriteCount;
            LastActivityDate = lastActivityDate;
            LastEditDate = lastEditDate;
            LastEditorDisplayName = lastEditorDisplayName;
            LastEditorUserId = lastEditorUserId;
            OwnerUserId = ownerUserId;
            ParentId = parentId;
            ViewCount = viewCount;
            _comments = new List<Comment>();
            _votes = new List<Vote>();
            _postLinks = new List<PostLink>();
            SetCreateDate(createDate);
        }

        
        public Result Close(DateTime closeDate)
        {
            if (ClosedDate != null)
                return Result.Failure("Post was already closed");

            ClosedDate = closeDate;

            Delete(closeDate);

            return Result.Success();
        }
    }
}
