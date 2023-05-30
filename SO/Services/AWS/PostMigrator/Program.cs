using Amazon.SQS;
using Amazon.SQS.Model;
using Contracts;
using Dapper;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.Json;

namespace PostMigrator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var messageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType",
                    new MessageAttributeValue
                    {
                        StringValue = nameof(PostAdded),
                        DataType = "String"
                    }
                }
            };

            var region = Amazon.RegionEndpoint.GetBySystemName("");
            IAmazonSQS sqsClient = new AmazonSQSClient("", "", region);

            var queueUrlResponse = await sqsClient.GetQueueUrlAsync("SO_Posts");
            if (queueUrlResponse == null || queueUrlResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Invalid AWS GetQueueUrlAsync response: {queueUrlResponse}");

            string connectionString = "";

            int lastId = 0;
            int fetchNext = 10;
            var count = 0;

            string sql = $@"
SELECT TOP ({fetchNext}) Id, Body, Title 
FROM Posts 
WHERE Id > @lastId
ORDER BY Id";

            while (true)
            {
                using SqlConnection connection = new(connectionString);

                var posts = connection.Query<PostAdded>(sql, new { lastId }).ToList();

                if (!posts.Any())
                    break;

                count += posts.Count;

                var messages = new List<SendMessageBatchRequestEntry>(fetchNext);
                foreach (var postDto in posts)
                {
                    messages.Add(new SendMessageBatchRequestEntry
                    {
                        MessageBody = JsonSerializer.Serialize(postDto),
                        Id = postDto.Id.ToString(),
                        MessageAttributes = messageAttributes
                    });
                }
                var request = new SendMessageBatchRequest(queueUrlResponse.QueueUrl, messages);

                var response = await sqsClient.SendMessageBatchAsync(request);
                if (response == null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception($"Invalid AWS SendMessageBatchAsync response: {response}");

                if (count % 10000 == 0) break;
                    //Console.WriteLine($"Items processed: {count}");

                lastId = posts.Last().Id;
            }
        }
    }
}