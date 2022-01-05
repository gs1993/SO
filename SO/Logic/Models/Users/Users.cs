using CSharpFunctionalExtensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logic.Models.Users
{
    public partial class Users : Entity
    {
        #region ctors
        protected Users()
        {

        }
        public Users(string aboutMe, int? age, DateTime creationDate, string displayName, DateTime lastAccessDate,
            string location, int reputation, int views, string websiteUrl, int createdPostCount, VoteSummary voteSummary)
        {
            AboutMe = aboutMe;
            Age = age;
            CreationDate = creationDate;
            DisplayName = displayName;
            LastAccessDate = lastAccessDate;
            Location = location;
            Reputation = reputation;
            Views = views;
            WebsiteUrl = websiteUrl;
            CreatedPostCount = createdPostCount;
            VoteSummary = voteSummary;
        } 
        #endregion

        #region Properties
        public string AboutMe { get; set; }
        public int? Age { get; set; }
        public DateTime CreationDate { get; set; }
        public string DisplayName { get; set; }
        public DateTime LastAccessDate { get; set; }
        public string Location { get; set; }
        public int Reputation { get; set; }
        public int Views { get; set; }
        public string WebsiteUrl { get; set; }
        [NotMapped]
        public int CreatedPostCount { get; set; }

        public VoteSummary VoteSummary { get; set; }
        #endregion


        public Result<Users> Create(string aboutMe, int? age, DateTime creationDate, string displayName, DateTime lastAccessDate,
            string location, int reputation, int views, string websiteUrl, int createdPostCount, VoteSummary voteSummary)
        {
            //TODO: add length validation

            if (creationDate > DateTime.UtcNow)
                return Result.Failure<Users>("Invalid user creation date");

            if (lastAccessDate > DateTime.UtcNow)
                return Result.Failure<Users>("Invalid user last access date");

            return Result.Success(new Users(aboutMe, age, creationDate, displayName, lastAccessDate, location, reputation, 
                views, websiteUrl, createdPostCount, voteSummary));
        }


        public Result SetCreatedPostCount(int createdPostCount)
        {
            if (createdPostCount < 0)
                return Result.Failure("View count cannot be less than zero");

            CreatedPostCount = createdPostCount;

            return Result.Success();
        }
    }
}
