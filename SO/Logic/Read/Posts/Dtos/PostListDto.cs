using Logic.Read.Posts.Models;
using System;

namespace Logic.Queries.Posts.Dtos
{
    public sealed class PostListDto
    {

        public PostListDto() { }
        public PostListDto(PostModel post)
        {
            Id = post.Id;
            Title = post.Title ?? string.Empty;
            Body = post.Body;
            AnswerCount = post.AnswerCount;
            CommentCount = post.CommentCount;
            Score = post.Score;
            ViewCount = post.ViewCount;
            CreationDate = post.CreateDate;
            IsClosed = post.ClosedDate != null;
            Tags = post.GetTagsArray();
            UserName = post.User?.DisplayName ?? string.Empty;
        }

        public int Id { get; init; }
        public int AnswerCount { get; init; }
        public bool IsClosed { get; init; }
        public int CommentCount { get; init; }
        public DateTime CreationDate { get; init; }
        public int Score { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }
        public int ViewCount { get; init; }
        public string[] Tags { get; init; }
        public string UserName { get; init; }
    }
}
