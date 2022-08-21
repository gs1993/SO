using System;

namespace Logic.BoundedContexts.Posts.Dtos
{
    public class PostListDto
    {
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
