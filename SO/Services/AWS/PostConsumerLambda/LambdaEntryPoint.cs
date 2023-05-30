using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Contracts;
using Nest;
using System.Text.Json;

namespace PostConsumerLambda
{
    public interface ILambdaEntryPoint
    {
        Task Handler(SQSEvent evnt, ILambdaLogger logger);
    }

    public class LambdaEntryPoint : ILambdaEntryPoint
    {
        private readonly IElasticClient _elasticClient;

        public LambdaEntryPoint(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task Handler(SQSEvent evnt, ILambdaLogger logger)
        {
            logger.LogInformation("Handler invoked");

            foreach (var record in evnt.Records)
            {
                var recordTypeAttribute = record.MessageAttributes.GetValueOrDefault("MessageType");
                if (recordTypeAttribute == null || string.IsNullOrWhiteSpace(recordTypeAttribute.StringValue))
                    throw new Exception("Invalid MessageType attribute");

                var recordType = recordTypeAttribute.StringValue;
                switch (recordType)
                {
                    case "PostAdded":
                        var postToAdd = JsonSerializer.Deserialize(record.Body, typeof(PostAdded)) as PostAdded;
                        await AddPost(postToAdd ?? throw new ArgumentNullException());
                        break;

                    default: throw new Exception($"Invalid record type: {recordType}");
                }
            }
        }

        private async Task AddPost(PostAdded post)
        {
            var response = await _elasticClient.IndexDocumentAsync(post);
            if (!response.IsValid)
                throw new Exception($"Failed to save object: {response.ServerError.Error}");
        }
    }
}
