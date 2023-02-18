using System;

namespace Logic.Read.Users.Models
{
    public sealed class UserModel
    {
        public int Id { get; init; }
        public bool IsDeleted { get; init; }
        public DateTime CreateDate { get; init; }
        public string DisplayName { get; init; }
        public DateTime LastAccessDate { get; init; }
        public string? Location { get; init; }
        public int Reputation { get; init; }
        public int Views { get; init; }
        public string? WebsiteUrl { get; init; }
        public int UpVotes { get; init; }
        public int DownVotes { get; init; }
        public string? AboutMe { get; init; }
        public int? Age { get; init; }
    }
}
