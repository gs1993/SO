using Api;
using IntegrationTests.Utils;
using Logic.BoundedContexts.Posts.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Posts
{
    public class PostControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;

        public PostControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Should_GetReturnPost_WhenCorrectId()
        {
            var response = await _client.GetAsync("/api/post/1");

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var postDetails = JsonConvert.DeserializeObject<PostDetailsDto>(responseJson);

            Assert.Equal(1, postDetails.Id);
            Assert.Equal("Test title", postDetails.Title);
        }
    }
}
