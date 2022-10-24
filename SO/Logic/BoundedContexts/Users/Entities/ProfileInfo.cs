using CSharpFunctionalExtensions;
using Logic.Utils;
using System.Collections.Generic;

namespace Logic.BoundedContexts.Users.Entities
{
    public class ProfileInfo : ValueObject
    {
        public string? AboutMe { get; private set; }
        public int? Age { get; private set; }
        public string? WebsiteUrl { get; private set; }
        public string? Location { get; private set; }

        protected ProfileInfo() { }
        public ProfileInfo(string? aboutMe, int? age, string? websiteUrl, string? location)
        {
            AboutMe = aboutMe;
            Age = age;
            WebsiteUrl = websiteUrl;
            Location = location;
        }

        public static Result<ProfileInfo, Error> Create(string? aboutMe, int? age, string? websiteUrl, string? location)
        {
            var trimmedAboutMe = aboutMe?.Trim();
            if (!string.IsNullOrEmpty(trimmedAboutMe))
            {
                if (trimmedAboutMe.Length > 500)
                    return Errors.General.InvalidLength(nameof(aboutMe));
            }

            if (age.HasValue && (age < 15 || age > 100))
                return Errors.General.InvalidValue(nameof(age));

            var trimmedWebsiteUrl = websiteUrl?.Trim();
            if (!string.IsNullOrEmpty(trimmedWebsiteUrl))
            {
                if (trimmedWebsiteUrl.Length > 100)
                    return Errors.General.InvalidLength(nameof(websiteUrl));
            }

            var trimmedLocation = location?.Trim();
            if (!string.IsNullOrEmpty(trimmedLocation))
            {
                if (trimmedLocation.Length > 150)
                    return Errors.General.InvalidLength(nameof(location));
            }

            return new ProfileInfo(trimmedAboutMe, age, trimmedWebsiteUrl, trimmedLocation);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AboutMe ?? string.Empty;
            yield return Age ?? 0;
            yield return Location ?? string.Empty;
            yield return WebsiteUrl ?? string.Empty;
        }
    }
}
