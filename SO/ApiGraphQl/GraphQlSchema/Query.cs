using Api.Args.Post;
using Dawn;
using FluentValidation;
using HotChocolate.Subscriptions;
using Logic.Queries.Posts.Dtos;
using Logic.Read.Posts.Dtos;
using Logic.Read.Posts.Queries;
using MediatR;

namespace ApiGraphQl.GraphQlSchema
{
    public class Query
    {
        public async Task<IReadOnlyList<PostListDto>> GetPostsPage(int offset, int limit,
            IMediator mediator, ITopicEventSender eventSender, IValidatorFactory validatorFactory)
        {
            var validationResult = validatorFactory.GetValidator<GetArgs>().Validate(new GetArgs
            {
                Limit = limit,
                Offset = offset
            });
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.ToString());

            var postListResult = await mediator.Send(new GetPostsPageQuery
                (
                    offset: offset,
                    limit: limit,
                    searchArgs: Array.Empty<SearchArgs>(),
                    sortArgs: null
                ));

            await eventSender.SendAsync("GetPostsPage", postListResult.Posts);

            return postListResult.Posts;
        }

        public async Task<IReadOnlyList<PostListDto>> GetLastest(int size,
            IMediator mediator, ITopicEventSender eventSender, IValidatorFactory validatorFactory)
        {
            var validationResult = validatorFactory.GetValidator<GetLastestArgs>().Validate(new GetLastestArgs
            {
                Size = size
            });
            if (!validationResult.IsValid)
                throw new ArgumentException(validationResult.ToString());

            var posts = await mediator.Send(new GetLastestPostsQuery
                (
                    size: size
                ));

            await eventSender.SendAsync("GetLastest", posts);

            return posts;
        }

        public async Task<PostDetailsDto> Get(int id,
            IMediator mediator, ITopicEventSender eventSender)
        {
            Guard.Argument(id).Positive();

            var post = await mediator.Send(new GetPostQuery
                (
                    id: id
                ));

            await eventSender.SendAsync("GetPost", post);

            return post;
        }
    }
}
