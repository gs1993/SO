using Logic.BoundedContexts.Users.Entities;
using Xunit;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace UnitTests.Logic.Users
{
    public class UserTests
    {
        private readonly User _sut;

        public UserTests()
        {
            _sut = User.Create("test", new DateTime(2022, 01, 19),
                ProfileInfo.Create(null, null, null, null).Value).Value;
        }

        [Fact]
        public void Should_BanReturnFaili_WhenUserIsNotActive()
        {
            // Arrange
            _sut.Ban(new DateTime(2022, 02, 20));

            // Act
            var result = _sut.Ban(new DateTime(2022, 02, 21));

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("User was already deleted", result.Error);
        }

        [Fact]
        public void Should_BanInsertDeleteDate_WhenBanActiveUser()
        {
            var result = _sut.Ban(new DateTime(2022, 02, 20));

            Assert.True(result.IsSuccess);
            Assert.Equal(new DateTime(2022, 02, 20), _sut.DeleteDate);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1")]
        [InlineData(" ")]
        [InlineData("  12  ")]
        [InlineData("too long displayyyyy nameeeeee, moreeeeeeeeeeeeeeeeeeeee than 50 charactersss")]
        public void Should_CreateFail_WhenDisplayNameIsInvalid(string invalidDisplayName)
        {
            var result = User.Create
            (
                displayName: invalidDisplayName,
                creationDate: _sut.CreateDate,
                profileInfo: _sut.ProfileInfo
            );

            Assert.True(result.IsFailure);
            Assert.Contains("displayName", result.Error.Message);
        }

        [Fact]
        public void Should_CreateFail_WhenProfileInfoIsNull()
        {

            var result = User.Create
            (
                displayName: _sut.DisplayName,
                creationDate: _sut.CreateDate,
                profileInfo: null
            );

            Assert.True(result.IsFailure);
            Assert.Contains("profileInfo", result.Error.Message);
        }
    }
}
