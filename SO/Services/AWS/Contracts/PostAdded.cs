using System.Text.Json.Serialization;

namespace Contracts
{
    public sealed class PostAdded : IMessage
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("body")]
        public string Body { get; init; }
        

        public string MessageTypeAttribute => nameof(PostAdded);
    }
}
