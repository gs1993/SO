using CSharpFunctionalExtensions;
using Logic.Models;
using Logic.Users.Entities;
using System;
using System.Collections.Generic;

namespace Logic.Posts.Entities
{
    public class Post : BaseEntity
    {
        #region Properties
        public string Title { get; private set; }
        public string Body { get; private set; }
        public int Score { get; private set; }
        public string? Tags { get; private set; }
        public int? AcceptedAnswerId { get; private set; }
        public int AnswerCount { get; private set; }
        public DateTime? ClosedDate { get; private set; }
        public int CommentCount { get; private set; }
        public DateTime? CommunityOwnedDate { get; private set; }
        public int? FavoriteCount { get; private set; }
        public DateTime LastActivityDate { get; private set; }
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
        #endregion

        #region ctors
        protected Post() { }
        private Post(string title, string body, DateTime createDate,
            int authorId, string authorName, string? tags, Post? parent)
        {
            Title = title;
            Body = body;
            Tags = tags;
            Score = 0;
            AcceptedAnswerId = null; //TODO: to ref type Comment
            AnswerCount = 0;
            ClosedDate = null;
            CommentCount = 0;
            CommunityOwnedDate = null;
            FavoriteCount = 0;
            LastActivityDate = createDate;
            LastEditorDisplayName = authorName;
            LastEditorUserId = authorId;
            OwnerUserId = authorId;
            ParentId = parent?.Id; //TODO: to ref type Post
            ViewCount = 0;
            PostType = new PostType(); //TODO: to value from in memory list
            _comments = new List<Comment>();
            _votes = new List<Vote>();
            _postLinks = new List<PostLink>();
            SetCreateDate(createDate);
        }

        public static Result<Post> Create(string title, string body, DateTime createDate,
            int authorId, string authorName, string? tags, Post? parent)
        {
            if()


            return new Post(title, body, createDate, authorId, authorName, tags, parent);
        }

        #endregion


        public Result AddComment(User user, string comment)
        {
            if (user == null)
                return Result.Failure("User cannot be null");

            if (string.IsNullOrWhiteSpace(comment))
                return Result.Failure("Comment cannot be empty");

            _comments.Add(new Comment(user.Id, comment));
            return Result.Success();
        }

        public Result UpVote(User user)
        {
            if (user == null)
                return Result.Failure("User cannot be null");

            _votes.Add(new Vote(this, user, +1));
            return Result.Success();
        }

        public Result DownVote(User user)
        {
            if (user == null)
                return Result.Failure("User cannot be null");

            _votes.Add(new Vote(this, user, -1));
            return Result.Success();
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
