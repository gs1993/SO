using Logic.BoundedContexts.Posts.Entities;
using Logic.BoundedContexts.Users.Entities;
using Logic.Utils;
using System.Text;
using Xunit;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace UnitTests.Logic.Posts
{
    public class PostTests
    {
        private readonly Post _sut;
        private readonly User _user;

        public PostTests()
        {
            _user = User.Create("test", new DateTime(2022, 01, 19), ProfileInfo.Create(null, null, null, null).Value).Value;
            _sut = Post.Create("Test title", "test loooooooooooooong booooodyyyyyyyyyyyyyyyyyyyyyy",
                new DateTime(2022, 01, 19), _user, null).Value;
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
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpVote(null));

            Assert.Equal("Value cannot be null. (Parameter 'user')", exception.Message);
            Assert.Equal(0, _sut.Score);
            Assert.Empty(_sut.Votes);
        }

        [Fact]
        public void Should_DownVoteFail_WhenUserIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.DownVote(null));

            Assert.Equal("Value cannot be null. (Parameter 'user')", exception.Message);
            Assert.Equal(0, _sut.Score);
            Assert.Empty(_sut.Votes);
        }

        [Fact]
        public void Should_AddCommentThrowAnException_WhenUserIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddComment(null, "Test comment"));

            Assert.Equal("Value cannot be null. (Parameter 'user')", exception.Message);
            Assert.Equal(0, _sut.Score);
            Assert.Empty(_sut.Votes);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        public void Should_AddCommentFail_WhenCommentIsInvalid(string comment)
        {
            var result = _sut.AddComment(_user, comment);

            Assert.True(result.IsFailure);
            Assert.Equal("Comment cannot be empty", result.Error);
        }

        [Fact]
        public void Should_AddCommentFail_WhenCommentIsTooLong()
        {
            var result = _sut.AddComment(_user, CreateString(10_001));

            Assert.True(result.IsFailure);
            Assert.Equal("comment length is invalid", result.Error);
        }

        [Fact]
        public void Should_AddCommentSucceed_WhenCommentIsMinLength()
        {
            var result = _sut.AddComment(_user, "1");

            Assert.True(result.IsSuccess);
            var addedComment = Assert.Single(_sut.Comments);
            Assert.Equal("1", addedComment.Text);
            Assert.Equal(0, addedComment.Score);
        }

        [Fact]
        public void Should_AddCommentSucceed_WhenCommentIsMaxLength()
        {
            var comment = CreateString(10_000);
            var result = _sut.AddComment(_user, comment);

            Assert.True(result.IsSuccess);
            var addedComment = Assert.Single(_sut.Comments);
            Assert.Equal(comment, addedComment.Text);
            Assert.Equal(0, addedComment.Score);
        }

        [Fact]
        public void Should_CreateSucceed_WhenAllPropertiesMinLength()
        {
            var result = Post.Create
           (
               title: CreateString(5),
               body: CreateString(50),
               tags: "",
               createDate: _sut.CreateDate,
               author: _user
           );

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public void Should_CreateSucceed_WhenTagsIsNull()
        {
            var result = Post.Create
            (
               tags: null,
               title: CreateString(5),
               body: CreateString(50),
               createDate: _sut.CreateDate,
               author: _user
            );

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public void Should_CreateSucceed_WhenAllPropertiesMaxLength()
        {
            var result = Post.Create
           (
               title: CreateString(250),
               body: CreateString(1000),
               tags: CreateString(100),
               createDate: _sut.CreateDate,
               author: _user
           );

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
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
                author: _user,
                tags: _sut.Tags
            );

            Assert.True(result.IsFailure);
            Assert.Contains("title", result.Error.Message);
        }

        [Fact]
        public void Should_CreateThrowAnException_WhenUserIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Post.Create
            (
               author: null,
               title: CreateString(250),
               body: CreateString(1000),
               tags: CreateString(100),
               createDate: _sut.CreateDate
            ));

            Assert.Equal("Value cannot be null. (Parameter 'author')", exception.Message);
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
                author: _user,
                tags: _sut.Tags
            );

            Assert.True(result.IsFailure);
            Assert.Contains("body", result.Error.Message);
        }

        [Fact]
        public void Should_CloseSucceed_WhenCloseDateIsValid()
        {
            var closeDate = new DateTime(2023, 10, 15);

            var result = _sut.Close(closeDate);

            Assert.True(result.IsSuccess);
            Assert.True(_sut.IsDeleted);
            Assert.Equal(closeDate, _sut.ClosedDate);
        }

        [Fact]
        public void Should_CloseFail_WhenPostWasAlreadyClosed()
        {
            var closeDate = new DateTime(2023, 10, 15);
            _sut.Close(closeDate);

            var result = _sut.Close(closeDate);

            Assert.True(result.IsFailure);
            Assert.Equal("Post already closed", result.Error);
        }


        private static string CreateString(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                // Append a random letter (e.g., 'A') or any character you want
                sb.Append('A');
            }
            return sb.ToString();
        }
    }
}
