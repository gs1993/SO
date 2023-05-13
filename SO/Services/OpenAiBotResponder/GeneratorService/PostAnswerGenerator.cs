using Logic.BoundedContexts.Posts.Entities;
using Logic.Contracts;
using Microsoft.EntityFrameworkCore.Metadata;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAiBotResponder.GeneratorService
{
    public class PostAnswerGenerator : IPostAnswerGenerator
    {
        private readonly OpenAIAPI _api;

        public PostAnswerGenerator(OpenAIAPI api)
        {
            _api = api;
        }

        public Task<PostAnswerResponse> GeneratePostAnswer(Post post)
        {
            


            throw new NotImplementedException();
        }
    }
}
