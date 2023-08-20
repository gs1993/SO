namespace Logic.Read.Posts.Dtos
{
    public sealed record SearchArgs
    {
        public string Field { get; init; }
        public SearchOperation Operation { get; init; }
        public string Value { get; init; }
    }

    public enum SearchOperation
    {
        Equals,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Contains,
        StartsWith
    }
}
