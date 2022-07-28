using AutoFixture;
using Logic.Posts.Entities;
using Logic.Users.Entities;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Logic.Posts.Commands
{
    public class AddCommentCommandHandlerTests
    {
        private static readonly Fixture Fixture = new();

        [Fact]
        public void testc()
        {
            int userId = 123;
            int postId = 456;

            var users = new List<User>
            {
                Fixture.Build<User>().WithAutoProperties().Create(),
                Fixture.Build<User>().WithAutoProperties().Create(),
                Fixture.Build<User>().With(u => u.Id, userId).Create()
            };

            var posts = new List<Post>
            {
                Fixture.Build<Post>().WithAutoProperties().Create(),
                Fixture.Build<Post>().WithAutoProperties().Create(),
                Fixture.Build<Post>()
                    .With(p => p.Id, postId)
                    .With(p => p.Comments, Array.Empty<Comment>())
                    .Create()
            };


        }
    }
}
