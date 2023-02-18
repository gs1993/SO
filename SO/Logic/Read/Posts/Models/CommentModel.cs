using System;
namespace Logic.Read.Posts.Models
{
    public sealed class CommentModel
    {
        public int Id { get; init; }
        public bool IsDeleted { get; init; }
        public DateTime CreateDate { get; init; }
        public int? Score { get; init; }
        public string Text { get; init; }
    }
}
