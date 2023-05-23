using ChatGptApi.Utils;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ChatGptApi.Services
{
    internal interface IChatGptClient
    {
        Task<Result<string>> GetResponse(string question, CancellationToken cancellationToken);
    }

    internal class ChatGptClient : IChatGptClient
    {
        private readonly HttpClient _httpClient;
        private readonly ChatGptApiSettings _apiSettings;
        private readonly ILogger<ChatGptClient> _logger;

        public ChatGptClient(HttpClient httpClient, ChatGptApiSettings apiSettings, ILogger<ChatGptClient> logger)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
            _logger = logger;
        }

        public async Task<Result<string>> GetResponse(string question, CancellationToken cancellationToken)
        {
            try
            {
                var request = new OpenAIRequest
                {
                    Model = _apiSettings.ModelName,
                    Messages = new OpenAIMessage[]
                    {
                        new OpenAIMessage
                        {
                            Role = _apiSettings.Role,
                            Content = question
                        }
                    },
                    Temperature = 0.7f
                };

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var response = await _httpClient
                    .PostAsync("chat/completions", content, cancellationToken)
                    .ConfigureAwait(false);

                using var responseStream = await response.Content
                    .ReadAsStreamAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await JsonSerializer
                        .DeserializeAsync<OpenAIErrorResponse>(responseStream, cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
                    
                    return Result.Failure<string>(errorResponse?.Error?.Message 
                        ?? $"Erorr response from OpenAI. Status code: {response.StatusCode}");
                }

                var openAIResponse = await JsonSerializer
                    .DeserializeAsync<OpenAIResponse>(responseStream, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                return openAIResponse?.Choices?
                    .FirstOrDefault()?.Message?.Content ?? string.Empty;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Result.Failure<string>("Chat GPT not responding");
            }
        }



        private class OpenAIResponse
        {
            [JsonPropertyName("choices")]
            public Choice[] Choices { get; init; }
        }

        private class Choice
        {
            [JsonPropertyName("message")]
            public OpenAIMessage Message { get; init; }

            [JsonPropertyName("finish_reason")]
            public string FinishReason { get; init; }
        }

        private class OpenAIRequest
        {
            [JsonPropertyName("model")]
            public string Model { get; init; }

            [JsonPropertyName("messages")]
            public OpenAIMessage[] Messages { get; init; }

            [JsonPropertyName("temperature")]
            public float Temperature { get; init; }
        }

        private class OpenAIMessage
        {
            [JsonPropertyName("role")]
            public string Role { get; init; }

            [JsonPropertyName("content")]
            public string Content { get; init; }
        }

        private class OpenAIErrorResponse
        {
            [JsonPropertyName("error")]
            public OpenAIError Error { get; init; }
        }

        private class OpenAIError
        {
            [JsonPropertyName("message")]
            public string Message { get; init; }

            [JsonPropertyName("type")]
            public string Type { get; init; }

            [JsonPropertyName("param")]
            public string Param { get; init; }

            [JsonPropertyName("code")]
            public string Code { get; init; }
        }
    }
}
