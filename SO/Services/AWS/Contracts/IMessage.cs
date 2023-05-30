using System.Text.Json.Serialization;

namespace Contracts
{
    public interface IMessage
    {
        [JsonIgnore]
        public string MessageTypeAttribute { get; }
    }
}
