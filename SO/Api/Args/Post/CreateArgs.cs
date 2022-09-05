namespace Api.Args.Post
{
    public record CreateArgs
    {
        public string Title { get; init; }
        public string Body { get; init; }
        public int AuthorId { get; init; }
        public string Tags { get; init; }
        public int? ParentId { get; init; }
    }
}
