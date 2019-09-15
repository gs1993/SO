using System;
using Logic.Utils;

namespace Logic.Dtos
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
        private const int BodyMaxLength = 150;

        public PostListDto(int id, string title, string body, int? answerCount, int? commentCount,
            DateTime creationDate, int score, int viewCount, DateTime? closedDate)
        {
            Id = id;
            Title = title;
            AnswerCount = answerCount ?? 0;
            CommentCount = commentCount ?? 0;
            CreationDate = creationDate;
            Score = score;
            ViewCount = viewCount;

            IsClosed = closedDate == null;
            ShortBody = body.Length > BodyMaxLength 
                ? $"{body.Substring(0, 150)}..."
                : body;
        }

        public int Id { get; set; }
        public int AnswerCount { get; set; }
        public bool IsClosed { get; set; }
        public int CommentCount { get; set; }
        public DateTime? CreationDate { get; set; }
        public int Score { get; set; }
        public string Title { get; set; }
        public string ShortBody { get; set; }
        public int ViewCount { get; set; }
    }
}
