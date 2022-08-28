using Logic.BoundedContexts.Posts.Entities;
using Logic.BoundedContexts.Users.Entities;
using Xunit;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace UnitTests.Logic.Posts
{
    public class PostTests
    {
        private readonly Post _sut = Post.Create("Test title", "test loooooooooooooong booooodyyyyyyyyyyyyyyyyyyyyyy", 
            new DateTime(2022, 01, 19), 1, "Test User", null, null).Value;

        private readonly User _user = User.Create("", null, new DateTime(2022, 01, 19), "", new DateTime(2022, 01, 19),
                "", 0, 0, "", 0, null).Value;

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
            var result =_sut.UpVote(null);

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
    }
}
