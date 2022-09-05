using Logic.BoundedContexts.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace UnitTests.Logic.Users
{
    public class ProfileInfoTests
    {
        private readonly ProfileInfo _sut = ProfileInfo.Create(null, null, null, null).Value;

        [Theory]
        [InlineData(-12)]
        [InlineData(0)]
        [InlineData(14)]
        [InlineData(101)]
        [InlineData(int.MaxValue)]
        public void Should_CreateFail_WhenAgeInvalid(int invalidAge)
        {
            var result = ProfileInfo.Create
            (
                age: invalidAge,
                aboutMe: _sut.AboutMe,
                location: _sut.Location,
                websiteUrl: _sut.WebsiteUrl
            );

            Assert.True(result.IsFailure);
            Assert.Contains("age", result.Error.Message);
        }

        [Fact]
        public void Should_CreateFail_WhenLocationIsTooLong()
        {
            var result = ProfileInfo.Create
            (
                location: "more than 150 characters more than 150 characters more than 150 characters more than 150 characters more than 150 characters more than 150 characters more than 150 characters ",
                age: _sut.Age,
                aboutMe: _sut.AboutMe,
                websiteUrl: _sut.WebsiteUrl
            );

            Assert.True(result.IsFailure);
            Assert.Contains("location", result.Error.Message);
        }

        [Fact]
        public void Should_CreateFail_WhenWebsiteUrlIsTooLong()
        {
            var result = ProfileInfo.Create
            (
                websiteUrl: "more than 100 characters more than 100 characters more than 100 characters more than 100 characters more than 100 characters",
                age: _sut.Age,
                aboutMe: _sut.AboutMe,
                location: _sut.Location
            );

            Assert.True(result.IsFailure);
            Assert.Contains("websiteUrl", result.Error.Message);
        }
    }

    public class VoteSummaryTests
    {
        private readonly VoteSummary _sut = VoteSummary.Create(1, 1).Value;

        [Fact]
        public void Should_CreateFail_WhenUpVotesIsBelowZero()
        {
            var result = VoteSummary.Create
            (
                upVotes: -1,
                downVotes: 1
            );

            Assert.True(result.IsFailure);
            Assert.Contains("UpVotes", result.Error);
        }

        [Fact]
        public void Should_CreateFail_WhenDownVotesIsBelowZero()
        {
            var result = VoteSummary.Create
            (
                downVotes: -1,
                upVotes: 1
            );

            Assert.True(result.IsFailure);
            Assert.Contains("DownVotes", result.Error);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(10, 0, 10)]
        [InlineData(25, 24, 49)]
        [InlineData(1000, 1000, 2000)]
        public void Should_VoteCount_BeSumOfUpVotesAndDownVotes(int upVotes, int downVotes, int sum)
        {
            var result = VoteSummary.Create
            (
                downVotes: upVotes,
                upVotes: downVotes
            );

            Assert.True(result.IsSuccess);
            Assert.Equal(sum, result.Value.VoteCount);
        }
    }
}
