using System;
using Logic.Utils;

namespace Logic.Dtos
{
    public class PostListDto
    {
        public PostListDto(string title, int? answerCount, int? commentCount,
            DateTime creationDate, int score, int viewCount, DateTime? closedDate)
        {
            Title = title;
            AnswerCount = answerCount ?? 0;
            CommentCount = commentCount ?? 0;
            CreationDate = creationDate.ToString(Consts.ShortDateFormat);
            Score = score;
            ViewCount = viewCount;

            IsClosed = closedDate == null;
        }

        public int AnswerCount { get; set; }
        public bool IsClosed { get; set; }
        public int CommentCount { get; set; }
        public string CreationDate { get; set; }
        public int Score { get; set; }
        public string Title { get; set; }
        public int ViewCount { get; set; }
    }
}