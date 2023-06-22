namespace CdcWorkerService.Db.Models
{
    internal class PostCT
    {
        public OperationEnum Operation { get; init; }
        public int Id { get; init; }
        public string? Title { get; init; }
        public string? Body { get; init; }
        public int? Score { get; init; }
        public string? Tags { get; init; }
        public int? AnswerCount { get; init; }
        public DateTime? ClosedDate { get; init; }
        public int? CommentCount { get; init; }
        public DateTime? CreateDate { get; init; }
        public DateTime? CommunityOwnedDate { get; init; }
        public int? FavoriteCount { get; init; }
        public DateTime? LastActivityDate { get; init; }
        public string? LastEditorDisplayName { get; init; }
        public int? ViewCount { get; init; }
        public bool? IsDeleted { get; init; }
    }
}
