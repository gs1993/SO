
using System;

namespace AdminPanel.Api
{
    public class LastUserDto
    {
        public DateTime CreationDate { get; }
        public string DisplayName { get; }


        public override string ToString()
        {
            return DisplayName;
        }
    }

    public class UserDetailsDto
    {
        public string AboutMe { get; }
        public int? Age { get; }
        public DateTime CreationDate { get; }
        public string DisplayName { get; }
        public DateTime LastAccessDate { get; }
        public string Location { get; }
        public int Reputation { get; }
        public int Views { get; }
        public string WebsiteUrl { get; }
        public int CreatedPostCount { get; }
        public int VoteCount { get; }
        public int UpVotes { get; }
        public int DownVotes { get; }
    }
}
