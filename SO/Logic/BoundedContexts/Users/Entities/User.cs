using CSharpFunctionalExtensions;
using Logic.Utils;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logic.BoundedContexts.Users.Entities
{
    public partial class User : BaseEntity
    {
        #region Properties
        public string AboutMe { get; set; }
        public int? Age { get; set; }
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

        #region ctors
        protected User() { }
        public User(string aboutMe, int? age, DateTime creationDate, string displayName, DateTime lastAccessDate,
            string location, int reputation, int views, string websiteUrl, int createdPostCount, VoteSummary voteSummary)
        {
            AboutMe = aboutMe;
            Age = age;
            DisplayName = displayName;
            LastAccessDate = lastAccessDate;
            Location = location;
            Reputation = reputation;
            Views = views;
            WebsiteUrl = websiteUrl;
            CreatedPostCount = createdPostCount;
            VoteSummary = voteSummary;
            SetCreateDate(creationDate);
        }

        public static Result<User> Create(string aboutMe, int? age, DateTime creationDate, string displayName, DateTime lastAccessDate,
            string location, int reputation, int views, string websiteUrl, int createdPostCount, VoteSummary voteSummary)
        {
            //TODO: add length validation

            if (creationDate > DateTime.UtcNow)
                return Result.Failure<User>("Invalid user creation date");

            if (lastAccessDate > DateTime.UtcNow)
                return Result.Failure<User>("Invalid user last access date");

            return Result.Success(new User(aboutMe, age, creationDate, displayName, lastAccessDate, location, reputation,
                views, websiteUrl, createdPostCount, voteSummary));
        }
        #endregion




        public Result SetCreatedPostCount(int createdPostCount)
        {
            if (createdPostCount < 0)
                return Result.Failure("View count cannot be less than zero");

            CreatedPostCount = createdPostCount;

            return Result.Success();
        }

        public void Ban(DateTime banDate)
        {
            if (IsDeleted)
                throw new InvalidOperationException("User not exists");

            Delete(banDate);
        }
    }
}
