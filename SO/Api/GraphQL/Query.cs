using Dawn;
using HotChocolate;
using HotChocolate.Subscriptions;
using Logic.BoundedContexts.Posts.Dtos;
using Logic.BoundedContexts.Posts.Queries;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.GraphQL
{
    public class Query
    {
        public async Task<IReadOnlyList<PostListDto>> GetPostsPage(
            [Service] IMediator mediator,
            [Service] ITopicEventSender eventSender,
            int offset, int limit)
        {
            Guard.Argument(offset).NotNegative();
            Guard.Argument(limit).Positive();

            var posts = await mediator.Send(new GetPostsPageQuery
                (
                    offset: offset,
                    limit: limit
                ));

            await eventSender.SendAsync("GetPostsPage", posts);

            return posts;
        }

        public async Task<IReadOnlyList<PostListDto>> GetLastest(
            [Service] IMediator mediator,
            [Service] ITopicEventSender eventSender,
            int size)
        {
            Guard.Argument(size).Positive();

            var posts = await mediator.Send(new GetLastestPostsQuery
                (
                    size: size
                ));

            await eventSender.SendAsync("GetLastest", posts);

            return posts;
        }

        public async Task<PostDetailsDto> Get(
            [Service] IMediator mediator,
            [Service] ITopicEventSender eventSender,
            int id)
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
