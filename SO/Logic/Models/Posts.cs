using CSharpFunctionalExtensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Logic.Models
{
    public partial class Posts : Entity
    {
        #region Properties
        public int? AcceptedAnswerId { get; set; }
        public int? AnswerCount { get; set; }
        public string Body { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int? CommentCount { get; set; }
        public DateTime? CommunityOwnedDate { get; set; }
        public DateTime CreationDate { get; set; }
        public int? FavoriteCount { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string LastEditorDisplayName { get; set; }
        public int? LastEditorUserId { get; set; }
        public int? OwnerUserId { get; set; }
        public int? ParentId { get; set; }
        public int PostTypeId { get; set; }
        public int Score { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public int ViewCount { get; set; } 

        public virtual ICollection<Comments> Comments { get; set; }
        #endregion


        public Result Close()
        {
            if (ClosedDate != null)
                return Result.Fail("Post was already closed");

            ClosedDate = DateTime.UtcNow;

            return Result.Ok();
        }
    }
}
