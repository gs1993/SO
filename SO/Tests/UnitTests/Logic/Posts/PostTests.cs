using Logic.Posts.Entities;
using Xunit;

namespace UnitTests.Logic.Posts
{
    public class PostTests
    {
        private readonly Post _sut = Post.Create().Value;

        [Fact]
        public void AddComment_ReturnsFail_WhenUserIsNull()
        {

        }

    }
}
