using CSharpFunctionalExtensions;
using Logic.BoundedContexts.Users.Entities;
using Logic.Utils;
using Logic.Utils.Db;
using System;
using System.Collections.Generic;

namespace Logic.BoundedContexts.Posts.Entities
{
    public class Post : BaseEntity
    {
        #region Properties
        public string? Title { get; private set; }
        public string Body { get; private set; }
        public int Score { get; private set; }
        public string? Tags { get; private set; }
        public int? AcceptedAnswerId { get; private set; }
        public int AnswerCount { get; private set; }
        public DateTime? ClosedDate { get; private set; }
        public int CommentCount { get; private set; }
        public DateTime? CommunityOwnedDate { get; private set; }
        public int FavoriteCount { get; private set; }
        public DateTime LastActivityDate { get; private set; }
        public string? LastEditorDisplayName { get; private set; }
        public int LastEditorUserId { get; private set; }
        public int OwnerUserId { get; private set; }
        public int ViewCount { get; private set; }

        public virtual PostType PostType { get; private set; }

        private readonly List<Comment> _comments = new();
        public virtual IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

        private readonly List<Vote> _votes = new();
        public virtual IReadOnlyList<Vote> Votes => _votes.AsReadOnly();

        private readonly List<PostLink> _postLinks = new();
        public virtual IReadOnlyList<PostLink> PostLinks => _postLinks.AsReadOnly();
        #endregion

        #region ctors
        protected Post() { }
        private Post(string title, string body, DateTime createDate,
            int authorId, string authorName, string? tags)
        {
            Title = title;
            Body = body;
            LastActivityDate = createDate;
            LastEditorDisplayName = authorName;
            LastEditorUserId = authorId;
            OwnerUserId = authorId;
            Tags = tags;
            AcceptedAnswerId = null;
            AnswerCount = 0;
            ClosedDate = null;
            CommentCount = 0;
            CommunityOwnedDate = null;
            FavoriteCount = 0;
            ViewCount = 0;
            Score = 0;
            CommentCount = 0;

            PostType = PostType.Question;
        }

        public static Result<Post, Error> Create(string title, string body, DateTime createDate,
            int authorId, string authorName, string? tags)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Errors.General.ValueIsRequired(nameof(title));
            var trimmedTitle = title.Trim();
            if (trimmedTitle.Length < 5 || trimmedTitle.Length > 250)
                return Errors.General.InvalidLength(nameof(title));

            if (string.IsNullOrWhiteSpace(body))
                return Errors.General.ValueIsRequired(nameof(body));
            var trimmedBody = body.Trim();
            if (trimmedBody.Length < 50 || trimmedBody.Length > 1000)
                return Errors.General.InvalidLength(nameof(body));

            var trimmedTags = tags?.Trim() ?? string.Empty;
            if (trimmedTags?.Length > 100)
                return Errors.General.InvalidLength(nameof(tags));

            if (createDate == DateTime.MinValue)
                return Errors.General.ValueIsRequired(nameof(createDate));

            if (authorId < 0)
                return Errors.General.ValueIsRequired(nameof(authorId));

            if (string.IsNullOrWhiteSpace(authorName))
                return Errors.General.ValueIsRequired(nameof(authorName));

            return new Post(trimmedTitle, trimmedBody, createDate, authorId, authorName, trimmedTags);
        }
        #endregion

        public Result AddComment(User user, string comment)
        {
            if (user == null)
                return Errors.General.ValueIsRequired(nameof(user));
            if (string.IsNullOrWhiteSpace(comment))
                return Errors.Post.CommentIsRequired();


            _comments.Add(new Comment(user.Id, comment));

            return Result.Success();
        }

        public Result UpVote(User user)
        {
            if (user == null)
                return Errors.General.ValueIsRequired(nameof(user));

            AddVote(user, 1);

            return Result.Success();
        }

        public Result DownVote(User user)
        {
            if (user == null)
                return Errors.General.ValueIsRequired(nameof(user));

            AddVote(user, -1);

            return Result.Success();
        }

        public Result Close(DateTime closeDate)
        {
            if (ClosedDate != null)
                return Errors.Post.AlreadyClosed();

            ClosedDate = closeDate;
            Delete(closeDate);

            return Result.Success();
        }

        private void AddVote(User user, int voteScore)
        {
            ArgumentNullException.ThrowIfNull(user);

            _votes.Add(new Vote(this, user, voteScore));
            Score += voteScore;
        }
    }
}
