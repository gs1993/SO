using FastEndpoints;
using Logic.Contracts;
using Microsoft.AspNetCore.Authorization;
using PredictionEngineApi.Dtos;

namespace PredictionEngineApi.Endpoints
{
    [HttpGet("prediction"), AllowAnonymous]
    public class GetPrediction : Endpoint<GetPredictionRequest, PredictionResponse>
    {
        private readonly IAntySpamPredictionService _antySpamPredictionService;

        public GetPrediction(IAntySpamPredictionService antySpamPredictionService)
        {
            _antySpamPredictionService = antySpamPredictionService;
        }

        public override async Task HandleAsync(GetPredictionRequest req, CancellationToken ct)
        {
            var prediction = _antySpamPredictionService.Predict(req.Body);

            await SendOkAsync(new PredictionResponse(
                prediction.Prediction == IsSpamPredictionEnum.Spam, 
                prediction.ConfidenceLevel), ct);   
        }
    }
}
