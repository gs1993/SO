namespace Api.Args.User
{
    public sealed record CreateUserArgs
    {
        public string AboutMe { get; init; }
        public int? Age { get; init; }
        public string DisplayName { get; init; }
        public string Location { get; init; }
        public string WebsiteUrl { get; init; }
    }
}
