using Logic.BoundedContexts.Posts.Entities;
using Logic.Utils;
using System;

namespace IntegrationTests.Utils
{
    internal class TestDataInitializer
    {
        internal static void Seed(DatabaseContext context)
        {
            var post = Post.Create("Test title", "test loooooooooooooong booooodyyyyyyyyyyyyyyyyyyyyyy",
                new DateTime(2022, 01, 19), 1, "Test User", null).Value;
            context.Posts.Add(post);

            context.SaveChanges();
        }
    }
}
