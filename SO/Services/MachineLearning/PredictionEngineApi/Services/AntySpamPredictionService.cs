using Logic.Contracts;
using Microsoft.ML;

namespace PredictionEngineApi.Services
{
    public class AntySpamPredictionService : IAntySpamPredictionService
    {
        private readonly PredictionEngine<InputData, IsSpamPredictionOutput> _predictionEngine;

        public AntySpamPredictionService(MLContext mlContext, ITransformer model)
        {
            _predictionEngine = mlContext.Model.CreatePredictionEngine<InputData, IsSpamPredictionOutput>(model);
        }

        public PrecictionResponse Predict(string postBody)
        {
            var inputData = new InputData
            {
                Body = postBody
            };
            var prediction = _predictionEngine.Predict(inputData);
            if (prediction == null)
                return new PrecictionResponse(IsSpamPredictionEnum.Inconclusive, default);

            int postScorePredictionValue = (int)Math.Round(prediction.PredictedLabel);

            var predictionEnum = Enum.IsDefined(typeof(IsSpamPredictionEnum), postScorePredictionValue)
                ? (IsSpamPredictionEnum)postScorePredictionValue
                : IsSpamPredictionEnum.Inconclusive;

            return new PrecictionResponse(predictionEnum, prediction.ConfidenceLevel);
        }

        private class IsSpamPredictionOutput
        {
            public float PredictedLabel { get; init; }
            public float ConfidenceLevel { get; init; }
        }

        private class InputData
        {
            public string Body { get; init; }
            public float Label { get; init; }
        }
    }
}
