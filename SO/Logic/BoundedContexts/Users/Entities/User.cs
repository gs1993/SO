using CSharpFunctionalExtensions;
using Logic.BoundedContexts.Posts.Dtos;
using Logic.Utils;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logic.BoundedContexts.Users.Entities
{
    public partial class User : BaseEntity
    {
        #region Properties
        public string AboutMe { get; private set; }
        public int? Age { get; private set; }
        public string DisplayName { get; private set; }
        public DateTime LastAccessDate { get; private set; }
        public string? Location { get; private set; }
        public int Reputation { get; private set; }
        public int Views { get; private set; }
        public string? WebsiteUrl { get; private set; }

        [NotMapped]
        public int CreatedPostCount { get; private set; }

        public VoteSummary VoteSummary { get; private set; }
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
                return Errors.General.InvalidValue(nameof(createdPostCount));

            CreatedPostCount = createdPostCount;

            return Result.Success();
        }

        public Result Ban(DateTime banDate)
        {
            if (banDate == DateTime.MinValue)
                return Errors.General.InvalidValue(nameof(banDate));
            if (IsDeleted)
                return Errors.User.AlreadyDeleted();

            Delete(banDate);

            return Result.Success();
        }
    }
}
