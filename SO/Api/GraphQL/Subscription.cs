using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System.Threading;
using System.Threading.Tasks;

namespace Api.GraphQL
{
    public class Subscription
    {
        [SubscribeAndResolve]
        public ValueTask<ISourceStream<int>> OnPostClosed(
            [Service] ITopicEventReceiver eventReceiver, CancellationToken cancellationToken)
        {
            return eventReceiver.SubscribeAsync<string, int>("PostClosed", cancellationToken);
        }
    }
}
