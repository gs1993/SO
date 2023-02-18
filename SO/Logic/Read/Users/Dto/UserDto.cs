using System;

namespace Logic.BoundedContexts.Users.Dto
{
    public class UserDto
    {
        public int Id { get; init; }
        public DateTime CreateDate { get; init; }
        public DateTime? LastUpdateDate { get; init; }
        public string DisplayName { get; init; }
        public DateTime LastAccessDate { get; init; }
        public int Reputation { get; init; }
        public int Views { get; init; }
        public int UpVotes { get; init; }
        public int DownVotes { get; init; }
        public string? AboutMe { get; init; }
        public int? Age { get; init; }
        public string? WebsiteUrl { get; init; }
        public string? Location { get; init; }
    }
}
