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
        public string? AboutMe { get; private set; }
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
        public User(string displayName, DateTime creationDate, string? aboutMe, int? age, string? location, string? websiteUrl)
        {
            AboutMe = aboutMe;
            Age = age;
            DisplayName = displayName;
            SetCreateDate(creationDate);
            LastAccessDate = creationDate;
            Location = location;
            WebsiteUrl = websiteUrl;
            Reputation = 0;
            Views = 0;
            CreatedPostCount = 0;
            VoteSummary = VoteSummary.Create(0, 0).Value;
        }

        public static Result<User, Error> Create(string displayName, DateTime creationDate, string? aboutMe, int? age, string? location, string? websiteUrl)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                return Errors.General.ValueIsRequired(nameof(displayName));
            var trimmedDisplayName = displayName.Trim();
            if (displayName.Length < 3 || displayName.Length > 50)
                return Errors.General.InvalidLength(nameof(displayName));

            if (creationDate == DateTime.MinValue)
                return Errors.General.InvalidValue(nameof(creationDate));

            var trimmedAboutMe = aboutMe?.Trim();
            if (!string.IsNullOrEmpty(trimmedAboutMe))
            {
                if (trimmedAboutMe.Length > 500)
                    return Errors.General.InvalidLength(nameof(aboutMe));
            }

            if (age.HasValue && (age < 15 || age > 100))
                return Errors.General.InvalidValue(nameof(age));

            var trimmedLocation = location?.Trim();
            if (!string.IsNullOrEmpty(trimmedLocation))
            {
                if (trimmedLocation.Length > 150)
                    return Errors.General.InvalidLength(nameof(location));
            }

            var trimmedWebsiteUrl = websiteUrl?.Trim();
            if (!string.IsNullOrEmpty(trimmedWebsiteUrl))
            {
                if (trimmedWebsiteUrl.Length > 100)
                    return Errors.General.InvalidLength(nameof(websiteUrl));
            }

            return new User(trimmedDisplayName, creationDate, trimmedAboutMe, age, trimmedLocation, trimmedWebsiteUrl);
        }
        #endregion

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
