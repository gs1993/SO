﻿using Api;
using IntegrationTests.Utils;
using Logic.BoundedContexts.Users.Dto;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Users
{
    public class UserControllerIntegrationTests : ControllerIntegrationTestsBase
    {
        private const int User2Id = 2;

        public UserControllerIntegrationTests(TestingWebAppFactory<Program> factory)
            : base(factory) { }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        [InlineData(5, 4)]
        [InlineData(100, 4)]
        public async Task Should_GetLastReturnExpectedAmountOfItems_WhenTableHasFourItems(int size, int expectedItemsCount)
        {
            // Arrange
            var query = new Dictionary<string, string>()
            {
                ["Size"] = size.ToString()
            };
            var uri = QueryHelpers.AddQueryString("/api/User/GetLast", query);

            // Act
            var response = await GetAndDeserializeResponse<IReadOnlyList<LastUserDto>>(uri);

            // Assert
            Assert.Equal(expectedItemsCount, response.Count);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1001)]
        public async Task Should_GetLastReturnBadRequest_WhenSizeIsIncorrect(int incorrectSize)
        {
            // Arrange
            var query = new Dictionary<string, string>()
            {
                ["Size"] = incorrectSize.ToString()
            };
            var uri = QueryHelpers.AddQueryString("/api/User/GetLast", query);

            // Act
            var response = await HttpClient.GetAsync(uri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_GetReturnUser_WhenItemExists()
        {
            // Act
            var userDetails = await GetAndDeserializeResponse<UserDetailsDto>($"/api/User/{User2Id}");

            // Assert
            Assert.Equal(User2Id, userDetails.Id);
            Assert.Equal("Test User 2", userDetails.DisplayName);
        }

        [Fact]
        public async Task Should_GetReturnNotFound_WhenItemDoesNotExists()
        {
            // Act
            var response = await HttpClient.GetAsync("/api/User/5");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_PermaBanRemoveUser_WhenUserIsActive()
        {
            // Act
            var response = await HttpClient.PutAsync($"/api/User/PermaBan/{User2Id}", new StringContent(""));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var notFoundResponse = await HttpClient.GetAsync($"/api/User/{User2Id}");
            Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);
        }

        [Fact]
        public async Task Should_PermaBanFail_WhenUserIsNotActive()
        {
            // Act
            var response1 = await HttpClient.PutAsync($"/api/User/PermaBan/{User2Id}", new StringContent(""));
            var response2 = await HttpClient.PutAsync($"/api/User/PermaBan/{User2Id}", new StringContent(""));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);

            var notFoundResponse = await HttpClient.GetAsync($"/api/User/{User2Id}");
            Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(50000)]
        public async Task Should_PermaBanFail_WhenUserDoesNotExists(int invalidId)
        {
            // Act
            var response = await HttpClient.PutAsync($"/api/User/PermaBan/{invalidId}", new StringContent(""));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
