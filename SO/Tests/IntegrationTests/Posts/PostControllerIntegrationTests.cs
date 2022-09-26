using Api;
using Api.Args.Post;
using IntegrationTests.Utils;
using Logic.BoundedContexts.Posts.Dtos;
using Newtonsoft.Json;
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

        [Fact]
        public async Task Should_GetReturnPost_WhenCorrectId()
        {
            var response = await HttpClient.GetAsync("/api/Post/2");

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var postDetails = JsonConvert.DeserializeObject<PostDetailsDto>(responseJson);

            Assert.Equal(2, postDetails.Id);
            Assert.Equal("Test title 2", postDetails.Title);
            Assert.Empty(postDetails.Comments);
        }

        [Fact]
        public async Task Should_AddCommentSucceed_WhenArgsAreCorrect()
        {
            var request = new AddCommentArgs
            { 
                UserId = 1,
                Comment = "Test comment"
            };
            var json = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PutAsync("/api/Post/AddComment/2", requestContent);
            response.EnsureSuccessStatusCode();

            var getResponse = await HttpClient.GetAsync("/api/Post/2");
            getResponse.EnsureSuccessStatusCode();

            var responseJson = await getResponse.Content.ReadAsStringAsync();
            var postDetails = JsonConvert.DeserializeObject<PostDetailsDto>(responseJson);

            Assert.Single(postDetails.Comments);
        }
    }
}
