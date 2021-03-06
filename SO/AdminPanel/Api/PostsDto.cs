﻿using System;

namespace AdminPanel.Api
{
    public class PostDetailsDto
    {
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
        public int Score { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public int ViewCount { get; set; }
    }

    public class PostListDto
    {
        public int Id { get; set; }
        public int AnswerCount { get; set; }
        public bool IsClosed { get; set; }
        public int CommentCount { get; set; }
        public string CreationDate { get; set; }
        public int Score { get; set; }
        public string Title { get; set; }
        public int ViewCount { get; set; }
    }
}
