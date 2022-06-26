namespace Api.Args.Post
{
    public record AddCommentArgs
    {
        public int UserId { get; init; }
        public string Comment { get; init; }
    }
}
