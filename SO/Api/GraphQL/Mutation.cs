﻿using HotChocolate;
using HotChocolate.Subscriptions;
using Logic.BoundedContexts.Posts.Commands;
using MediatR;
using System.Threading.Tasks;

namespace Api.GraphQL
{
    public class Mutation
    {
        public async Task<int> ClosePost([Service] IMediator mediator, 
            [Service] ITopicEventSender eventSender, int id)
        {
            var result = await mediator.Send(new ClosePostCommand(id));
            if (result.IsSuccess)
                await eventSender.SendAsync("PostClosed", id);

            return id;
        }
    }
}