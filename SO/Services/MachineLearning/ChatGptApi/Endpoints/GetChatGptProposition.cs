using ChatGptApi.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using PredictionEngineApi.Dtos;

namespace ChatGptApi.Endpoints
{
    [HttpGet("chatGptProposition"), AllowAnonymous]
    internal class GetChatGptProposition : Endpoint<GetChatGptPropositionRequest, GetChatGptPropositionResponse>
    {
        private readonly IChatGptClient _chatGptClient;

        public GetChatGptProposition(IChatGptClient chatGptClient)
        {
            _chatGptClient = chatGptClient ?? throw new ArgumentNullException(nameof(chatGptClient));
        }

        public override async Task HandleAsync(GetChatGptPropositionRequest req, CancellationToken ct)
        {
            var prediction = await _chatGptClient.GetResponse(req.Body, ct);
            if (prediction.IsFailure)
                await SendErrorsAsync(500, ct);

            await SendOkAsync(new GetChatGptPropositionResponse(prediction.Value), ct);
        }
    }
}
