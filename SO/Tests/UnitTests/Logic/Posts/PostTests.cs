using Logic.BoundedContexts.Posts.Entities;
using Logic.BoundedContexts.Users.Entities;
using Xunit;

#pragma warning disable CS8604 // Possible null reference argument.

namespace UnitTests.Logic.Posts
{
    public class PostTests
    {
        private readonly Post _sut;
        private readonly User _user;

        public PostTests()
        {
            _user = User.Create("test", new DateTime(2022, 01, 19), null, null, null, null).Value;
            _sut = Post.Create("Test title", "test loooooooooooooong booooodyyyyyyyyyyyyyyyyyyyyyy",
                new DateTime(2022, 01, 19), 1, "Test User", null).Value;
        }

        [Fact]
        public void Should_ScoreBeFour_WhenUpVotedFourTimes()
        {
            _sut.UpVote(_user);
            _sut.UpVote(_user);
            _sut.UpVote(_user);
            _sut.UpVote(_user);

            Assert.Equal(4, _sut.Score);
            Assert.NotNull(_sut.Votes);
            Assert.Equal(4, _sut.Votes.Count);
            Assert.True(_sut.Votes.All(x => x.BountyAmount == 1));
        }

        [Fact]
        public void Should_ScoreBeZero_WhenPostGotTwoUpVotesAndTwoDownVotes()
        {
            _sut.UpVote(_user);
            _sut.DownVote(_user);
            _sut.UpVote(_user);
            _sut.DownVote(_user);

            Assert.Equal(0, _sut.Score);
            Assert.NotNull(_sut.Votes);
            Assert.Equal(4, _sut.Votes.Count);
            Assert.Equal(2, _sut.Votes.Count(x => x.BountyAmount == 1));
            Assert.Equal(2, _sut.Votes.Count(x => x.BountyAmount == -1));
        }

        [Fact]
        public void Should_UpVoteFail_WhenUserIsNull()
        {
            var result = _sut.UpVote(null);

            Assert.NotNull(result.Error);
            Assert.Equal("user is required", result.Error);
            Assert.Equal(0, _sut.Score);
            Assert.Empty(_sut.Votes);
        }

        [Fact]
        public void Should_DownVoteFail_WhenUserIsNull()
        {
            var result = _sut.DownVote(null);

            Assert.NotNull(result.Error);
            Assert.Equal("user is required", result.Error);
            Assert.Equal(0, _sut.Score);
            Assert.Empty(_sut.Votes);
        }

        [Fact]
        public void Should_AddCommentFail_WhenUserIsNull()
        {
            var result = _sut.AddComment(null, "Test comment");

            Assert.NotNull(result.Error);
            Assert.Equal("user is required", result.Error);
            Assert.Equal(0, _sut.Score);
            Assert.Empty(_sut.Votes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  1234  ")]
        [InlineData("1234")]
        public void Should_CreateFail_WhenTitleIsInvalid(string invalidTitle)
        {
            var result = Post.Create
            (
                title: invalidTitle,
                body: _sut.Body,
                createDate: _sut.CreateDate,
                authorId: _user.Id,
                authorName: _user.DisplayName,
                tags: _sut.Tags
            );

            Assert.True(result.IsFailure);
            Assert.Contains("title", result.Error.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("too short body")]
        public void Should_CreateFail_WhenBodyIsInvalid(string invalidBody)
        {
            var result = Post.Create
            (
                title: _sut.Title,
                body: invalidBody,
                createDate: _sut.CreateDate,
                authorId: _user.Id,
                authorName: _user.DisplayName,
                tags: _sut.Tags
            );

            Assert.True(result.IsFailure);
            Assert.Contains("body", result.Error.Message);
        }
    }
}
