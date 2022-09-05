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
        public string DisplayName { get; private set; }
        public DateTime LastAccessDate { get; private set; }
        public int Reputation { get; private set; }
        public int Views { get; private set; }

        [NotMapped]
        public int CreatedPostCount { get; private set; }

        public VoteSummary VoteSummary { get; private set; }
        public ProfileInfo ProfileInfo { get; private set; }
        #endregion

        #region ctors
        protected User() { }
        public User(string displayName, DateTime creationDate, ProfileInfo profileInfo)
        {
            DisplayName = displayName;
            SetCreateDate(creationDate);
            LastAccessDate = creationDate;
            Reputation = 0;
            Views = 0;
            CreatedPostCount = 0;
            VoteSummary = VoteSummary.Create(0, 0).Value;
            ProfileInfo = profileInfo;
        }

        public static Result<User, Error> Create(string displayName, DateTime creationDate, ProfileInfo profileInfo)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                return Errors.General.ValueIsRequired(nameof(displayName));
            var trimmedDisplayName = displayName.Trim();
            if (displayName.Length < 3 || displayName.Length > 50)
                return Errors.General.InvalidLength(nameof(displayName));

            if (creationDate == DateTime.MinValue)
                return Errors.General.InvalidValue(nameof(creationDate));

            if (profileInfo == null)
                return Errors.General.InvalidValue(nameof(profileInfo));

            return new User(trimmedDisplayName, creationDate, profileInfo);
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
