namespace Api.Args.Post
{
    public sealed record AddCommentArgs
    {
        public int UserId { get; init; }
        public string Comment { get; init; }
    }
}
