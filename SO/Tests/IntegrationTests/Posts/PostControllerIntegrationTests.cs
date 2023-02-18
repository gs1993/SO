using Api;
using Api.Args.Post;
using IntegrationTests.Utils;
using Logic.Queries.Posts.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Posts
{
    public class PostControllerIntegrationTests : ControllerIntegrationTestsBase
    {
        private const int Post2Id = 2;
        private const int User1Id = 1;

        public PostControllerIntegrationTests(TestingWebAppFactory<Program> factory)
            : base(factory) { }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(10, 0)]
        [InlineData(0, 0)]
        [InlineData(1, -1)]
        [InlineData(0, 10001)]
        public async Task Should_GetListReturnBadRequest_WhenOffsetOrLimitAreIncorrect
            (int incorrectOffset, int incorrectLimit)
        {
            // Arrange
            var query = new Dictionary<string, string>()
            {
                ["Offset"] = incorrectOffset.ToString(),
                ["Limit"] = incorrectLimit.ToString()
            };
            var uri = QueryHelpers.AddQueryString("/api/Post", query);

            // Act
            var response = await HttpClient.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(0, 10, 4)]
        [InlineData(1, 10, 3)]
        [InlineData(2, 10, 2)]
        [InlineData(3, 10, 1)]
        [InlineData(4, 10, 0)]
        [InlineData(5, 10, 0)]
        [InlineData(3, 1, 1)]
        [InlineData(3, 2, 1)]
        [InlineData(1, 1, 1)]
        [InlineData(1, 3, 3)]
        public async Task Should_GetListReturnExpectedAmountOfItems_WhenTableHasFourItems
            (int offset, int limit, int expectedItemsCount)
        {
            // Arrange
            var query = new Dictionary<string, string>()
            {
                ["Offset"] = offset.ToString(),
                ["Limit"] = limit.ToString()
            };
            var uri = QueryHelpers.AddQueryString("/api/Post", query);

            // Act
            var response = await GetAndDeserializeResponse<PaginatedPostList>(uri);

            // Assert
            Assert.NotNull(response.Posts);
            Assert.Equal(expectedItemsCount, response.Posts.Count);
            Assert.Equal(4, response.Count);
        }

        [Fact]
        public async Task Should_GetReturnPost_WhenItemExists()
        {
            // Act
            var postDetails = await GetAndDeserializeResponse<PostDetailsDto>($"/api/Post/{Post2Id}");

            // Assert
            Assert.Equal(Post2Id, postDetails.Id);
            Assert.Equal("Test title 2", postDetails.Title);
            Assert.Empty(postDetails.Comments);
        }

        [Fact]
        public async Task Should_GetReturnNotFound_WhenItemDoesNotExists()
        {
            // Act
            var response = await HttpClient.GetAsync("/api/Post/2000");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1001)]
        public async Task Should_GetLastestReturnBadRequest_WhenSizeIsIncorrect(int incorrectSize)
        {
            // Arrange
            var query = new Dictionary<string, string>()
            {
                ["Size"] = incorrectSize.ToString()
            };
            var uri = QueryHelpers.AddQueryString("/api/Post/GetLastest", query);

            // Act
            var response = await HttpClient.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 4)]
        [InlineData(5, 4)]
        [InlineData(1000, 4)]
        public async Task Should_GetLastestReturnExpectedAmountOfItems_WhenTableHasFourItems(int size, int expectedItemsCount)
        {
            // Arrange
            var query = new Dictionary<string, string>()
            {
                ["Size"] = size.ToString()
            };
            var uri = QueryHelpers.AddQueryString("/api/Post/GetLastest", query);

            // Act
            var response = await GetAndDeserializeResponse<IReadOnlyList<PostListDto>>(uri);

            // Assert
            Assert.Equal(expectedItemsCount, response.Count);
        }

        [Fact]
        public async Task Should_CreateSucceed_WhenArgsAreCorrect()
        {
            // Arrange
            string postBody = "Loooooooooooooooooong body ...........................................................";
            var request = new CreateArgs
            {
                Title = "Test title",
                Body = postBody,
                AuthorId = User1Id,
                Tags = ""
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await HttpClient.PostAsync("/api/Post", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdPost = await GetAndDeserializeResponse<PostDetailsDto>("/api/Post/5");
            Assert.NotNull(createdPost);
            Assert.Equal("Test title", createdPost.Title);
            Assert.Equal(postBody, createdPost.Body);
            Assert.Equal("", createdPost.Tags);
        }

        [Fact]
        public async Task Should_CloseSucceed_WhenPostIsNotClosed()
        {
            // Act
            var response = await HttpClient.PutAsync($"/api/Post/Close/{Post2Id}", new StringContent(""));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var notFoundResponse = await HttpClient.GetAsync($"/api/Post/{Post2Id}");
            Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);
        }

        [Fact]
        public async Task Should_CloseFail_WhenPostDoesNotExists()
        {
            // Act
            var response = await HttpClient.PutAsync("/api/Post/Close/20000", new StringContent(""));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_CloseFail_WhenIdIsIncorrect()
        {
            // Act
            var response = await HttpClient.PutAsync("/api/Post/Close/0", new StringContent(""));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_AddCommentSucceed_WhenArgsAreCorrect()
        {
            // Arrange
            var request = new AddCommentArgs
            {
                UserId = User1Id,
                Comment = "Test comment"
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await HttpClient.PutAsync($"/api/Post/AddComment/{Post2Id}", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updatedPost = await GetAndDeserializeResponse<PostDetailsDto>($"/api/Post/{Post2Id}");
            Assert.NotNull(updatedPost);
            Assert.Single(updatedPost.Comments);
            Assert.Equal("Test comment", updatedPost.Comments.First().Text);
        }

        [Theory]
        [InlineData(-1, 1, "test")]
        [InlineData(1, -1, "test")]
        [InlineData(1, 1, "")]
        [InlineData(0, 1, "")]
        [InlineData(1, 0, "")]
        public async Task Should_AddCommentFail_WhenArgsAreIncorrect(int postId, int userId, string comment)
        {
            // Arrange
            var request = new AddCommentArgs
            {
                UserId = userId,
                Comment = comment
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await HttpClient.PutAsync($"/api/Post/AddComment/{postId}", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task Should_ScoreBeOne_WhenUpVoted()
        {
            // Arrange
            var request = new UpVoteArgs
            {
                UserId = User1Id
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await HttpClient.PutAsync($"/api/Post/UpVote/{Post2Id}", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updatedPost = await GetAndDeserializeResponse<PostDetailsDto>($"/api/Post/{Post2Id}");
            Assert.NotNull(updatedPost);
            Assert.Equal(1, updatedPost.Score);
        }

        [Fact]
        public async Task Should_ScoreBeThree_WhenUpVotedThreeTimes()
        {
            // Arrange
            var request = new UpVoteArgs
            {
                UserId = User1Id
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response1 = await HttpClient.PutAsync($"/api/Post/UpVote/{Post2Id}", requestContent);
            var response2 = await HttpClient.PutAsync($"/api/Post/UpVote/{Post2Id}", requestContent);
            var response3 = await HttpClient.PutAsync($"/api/Post/UpVote/{Post2Id}", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);

            var updatedPost = await GetAndDeserializeResponse<PostDetailsDto>($"/api/Post/{Post2Id}");
            Assert.NotNull(updatedPost);
            Assert.Equal(3, updatedPost.Score);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public async Task Should_UpVoteFail_WhenArgsAreIncorrect(int postId, int userId)
        {
            // Arrange
            var request = new UpVoteArgs
            {
                UserId = userId
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await HttpClient.PutAsync($"/api/Post/UpVote/{postId}", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public async Task Should_DownVoteFail_WhenArgsAreIncorrect(int postId, int userId)
        {
            // Arrange
            var request = new UpVoteArgs
            {
                UserId = userId
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await HttpClient.PutAsync($"/api/Post/DownVote/{postId}", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_ScoreBeMinusTwo_WhenDownVotedTwoTimes()
        {
            // Arrange
            var request = new UpVoteArgs
            {
                UserId = User1Id
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response1 = await HttpClient.PutAsync($"/api/Post/DownVote/{Post2Id}", requestContent);
            var response2 = await HttpClient.PutAsync($"/api/Post/DownVote/{Post2Id}", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            var updatedPost = await GetAndDeserializeResponse<PostDetailsDto>($"/api/Post/{Post2Id}");
            Assert.NotNull(updatedPost);
            Assert.Equal(-2, updatedPost.Score);
        }

        [Fact]
        public async Task Should_ScoreBeZero_WhenDownVotedTwoTimesAndUpVotedTwoTimes()
        {
            // Arrange
            var request = new UpVoteArgs
            {
                UserId = User1Id
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response1 = await HttpClient.PutAsync($"/api/Post/DownVote/{Post2Id}", requestContent);
            var response2 = await HttpClient.PutAsync($"/api/Post/DownVote/{Post2Id}", requestContent);
            var response3 = await HttpClient.PutAsync($"/api/Post/UpVote/{Post2Id}", requestContent);
            var response4 = await HttpClient.PutAsync($"/api/Post/UpVote/{Post2Id}", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response4.StatusCode);

            var updatedPost = await GetAndDeserializeResponse<PostDetailsDto>($"/api/Post/{Post2Id}");
            Assert.NotNull(updatedPost);
            Assert.Equal(0, updatedPost.Score);
        }
    }
}
