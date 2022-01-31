using System;

namespace Logic.Users.Dto
{
    public class UserDetailsDto
    {
        public int Id { get; set; }
        public string AboutMe { get; set; }
        public int? Age { get; set; }
        public DateTime CreationDate { get; set; }
        public string DisplayName { get; set; }
        public DateTime LastAccessDate { get; set; }
        public string Location { get; set; }
        public int Reputation { get; set; }
        public int Views { get; set; }
        public string WebsiteUrl { get; set; }
        public int CreatedPostCount { get; set; }
        public int VoteCount { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
    }
}
