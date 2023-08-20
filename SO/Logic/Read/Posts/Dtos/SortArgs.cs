namespace Logic.Read.Posts.Dtos
{
    public sealed record SortArgs
    {
        public string Field { get; init; }
        public SortDirection SortDirection { get; init; }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
