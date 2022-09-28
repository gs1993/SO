using Api;
using Api.Args.Post;
using IntegrationTests.Utils;
using Logic.BoundedContexts.Posts.Dtos;
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
        public PostControllerIntegrationTests(TestingWebAppFactory<Program> factory)
            : base(factory) { }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(10, 0)]
        [InlineData(0, 0)]
        [InlineData(1, -1)]
        [InlineData(0, 1001)]
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
            var response = await GetAndDeserializeResponse<IReadOnlyList<PostListDto>>(uri);

            // Assert
            Assert.Equal(expectedItemsCount, response.Count);
        }

        [Fact]
        public async Task Should_GetReturnPost_WhenItemExists()
        {
            // Act
            var postDetails = await GetAndDeserializeResponse<PostDetailsDto>("/api/Post/2");

            // Assert
            Assert.Equal(2, postDetails.Id);
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
        public async Task Should_GetLastestReturnExpectedAmountOfItems_WhenTableHasFourItems
            (int size, int expectedItemsCount)
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
                AuthorId = 1,
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
        public async Task Should_AddCommentSucceed_WhenArgsAreCorrect()
        {
            // Arrange
            var request = new AddCommentArgs
            { 
                UserId = 1,
                Comment = "Test comment"
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await HttpClient.PutAsync("/api/Post/AddComment/2", requestContent);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var updatedPost = await GetAndDeserializeResponse<PostDetailsDto>("/api/Post/2");
            Assert.NotNull(updatedPost);
            Assert.Single(updatedPost.Comments);
            Assert.Equal("Test comment", updatedPost.Comments.First().Text);
        }
    }
}
