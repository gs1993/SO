using Logic.BoundedContexts.Posts.Entities;
using System.Threading.Tasks;

namespace Logic.Contracts
{
    public interface IPostAnswerGenerator
    {
        public Task<PostAnswerResponse> GeneratePostAnswer(Post post);
    }

    public class PostAnswerResponse
    {
        public string Response { get; set; }
    }
}
