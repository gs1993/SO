namespace Logic.BoundedContexts.Users.Dto
{
    public class CreateUserDto
    {
        public string? AboutMe { get; init; }
        public int? Age { get; init; }
        public string DisplayName { get; init; }
        public string? Location { get; init; }
        public string? WebsiteUrl { get; init; }
    }
}
